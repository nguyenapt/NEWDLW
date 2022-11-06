module Netafim.Web {
    interface IInput {
        searchDealersUrl: string;
        blockId: number;
        contactFormLabel: string;
        directionsLabel: string;
        websiteLabel: string;
    }

    interface IQuery {
        blockId: number;
        latitude: number;
        longtitude: number;
        searchText: string;
    }
    export class DealerLocator {

        params: IInput;
        query: IQuery;
        prevInfoWindow: google.maps.InfoWindow;
        private ICON_BASE_URL: string = '/Content/images/';
        private jqueryElement: any;

        constructor(jqueryElement: any, parm: IInput) {

            this.params = parm;
            
            this.jqueryElement = jqueryElement;

            this.initState();

            this.registerEvents();

            this.query = <IQuery>{ blockId: this.params.blockId };

            this.loadDealers();
        }

        private initState() {
            //Hide map on mobile by default
            if ($('.nav-menu').is(':visible')) {
                $('.toggle-dealer-map', this.jqueryElement).removeClass('map-on');
                $('.toggle-dealer-map', this.jqueryElement).find('span').text($('.toggle-dealer-map', this.jqueryElement).attr('data-text-show'));
                $('.map-wrapper', this.jqueryElement).addClass('hide-map');
            }
        }

        private initMap(lat?: number, lng?: number) {

            //Map style, please dont change it if you don't know how it works
            var netafim_map_style = [
                { "featureType": "administrative", "elementType": "labels.text.fill", "stylers": [{ "color": "#444444" }] },
                { "featureType": "landscape", "elementType": "all", "stylers": [{ "color": "#f2f2f2" }] },
                { "featureType": "poi", "elementType": "all", "stylers": [{ "visibility": "off" }] },
                { "featureType": "road", "elementType": "all", "stylers": [{ "saturation": -100 }, { "lightness": 45 }] },
                { "featureType": "road.highway", "elementType": "all", "stylers": [{ "visibility": "simplified" }] },
                { "featureType": "road.highway", "elementType": "geometry.fill", "stylers": [{ "color": "#ffffff" }] },
                { "featureType": "road.arterial", "elementType": "labels.icon", "stylers": [{ "visibility": "off" }] },
                { "featureType": "transit", "elementType": "all", "stylers": [{ "visibility": "off" }] },
                { "featureType": "water", "elementType": "all", "stylers": [{ "color": "#dde6e8" }, { "visibility": "on" }] }
            ];
            
            //Creating a new maps
            var opt = <google.maps.MapOptions>{
                center: { lat: lat, lng: lng },
                clickableIcons: true,
                keyboardShortcuts: false,
                fullscreenControlOptions: false,
                zoom: 13,
                minZoom: 3,
                streetViewControl: false,
                disableDefaultUI: true, // a way to quickly hide all controls
                mapTypeControl: false,
                scaleControl: false,
                zoomControl: true,
                draggable: true,
                scrollwheel: false,
                styles: netafim_map_style
            };


            let map = new google.maps.Map(document.getElementById("netafim-dealer-map"), opt);

            return map;
        }

        private loadDealers() {
            var self = this;
            var query = this.query;
            query.searchText = $("#search-dealer", self.jqueryElement).val();

            $.ajax({
                type: "POST",
                url: "/" + this.params.searchDealersUrl,
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(query),
                success: function (data) {

                    self.populateDealers(data, query.latitude, query.longtitude);
                    
                    $('#btnSearchDealer').removeClass('searching');

                    if (query.searchText) {
                        dataLayer.push({
                            'event': 'dealerSearch',
                            'searchQuery': query.searchText,
                        });
                    }
                },
                complete: function() {
                    self.registerContactForm('.dealer-accordion');
                }
            });
        }

        private populateDealers(data: any, lat?: number, lng?: number) {

            var dealers = $.parseJSON(data);
            
            this.populateMap(dealers);
            this.populateViews(dealers);
        }

        private populateMap(dealers) {
            var self = this;
            let lat = this.query.latitude || 59.325;
            let lng = this.query.longtitude || 18.070;

            let map = this.initMap(lat, lng);

            let bounds = new google.maps.LatLngBounds();
            
            //Adding Markers to the existing maps
            for (var i = 0, length = dealers.length; i < length; i++) {
                var dealer = dealers[i];

                for (var j = 0; j < dealer.dealers.length; j++) {
                    this.createMaker(dealer.dealers[j], dealer.category_type, dealer.category_pin, dealer.category_color, map, bounds);
                }
            }
            
            if (dealers.length <= 0) // update fitbound
            {
                var position = new google.maps.LatLng(lat, lng);
                bounds.extend(position);
            }

            map.fitBounds(bounds);
        }

        private createMaker(dealer, type, pin, color, map, bounds) {

            var position = new google.maps.LatLng(dealer.lat, dealer.lng);

            var options: google.maps.MarkerOptions = {
                position: position,
                map: map,
                icon: pin,
                animation: google.maps.Animation.DROP,
                title: dealer.name
            };

            let infoWindow = new google.maps.InfoWindow();
            var marker = new google.maps.Marker(options);

            bounds.extend(position);

            let content = this.infoBox(type, dealer.logo, dealer.name, dealer.email, dealer.address, dealer.tel, color);
            google.maps.event.addListener(marker,
                'click',
                ((marker, content, infoWindow) => {
                    if (infoWindow) infoWindow.close();
                    return () => {
                        infoWindow.setContent(content);
                        if (this.prevInfoWindow != null) {
                            this.prevInfoWindow.close();
                        }
                        infoWindow.open(map, marker);

                        this.registerContactForm('.map-wrapper');

                        this.prevInfoWindow = infoWindow;
                        google.maps.event.addListener(map,
                            'click',
                            () => {
                                if (infoWindow) infoWindow.close();
                            });
                    };
                })(marker, content, infoWindow));
        }

        private populateViews(dealer_data) {
            $('.dealer-accordion').html(this.generateAccordionData(dealer_data));
            $('.accordion-container .accordion-item .accordion-item-title').on('click', function () {
                if ($(this).parent().hasClass('accordion-active')) {
                    $(this).parent().removeClass('accordion-active');
                } else {
                    $(this).parents('.accordion-container').find('.accordion-active').removeClass('accordion-active');
                    $(this).parent().addClass('accordion-active');
                }
            });
            //Open the first accordion item
            $('.dealer-accordion .accordion-item').first().addClass('accordion-active');
            $('.accordion-item-content-dealer').each(function () {
                var len = $(this).find('.dealer-wrapper').length;
                var divs = $(this).find('.dealer-wrapper');
                for (var i = 0; i < len; i += 3) {
                    divs.slice(i, i + 3).wrapAll('<div class="dealer-row"></div>');
                }
            });
        }

        private generateAccordionData(data) {
            var html = '';
            var totalFoundItems = 0;
            html += '<div class="container"><div class="row"><div class="col-xs-12"><div class="component-inner"><div class="accordion-container has-shadow-desktop"><div class="accordion-inner"><ul>';
            if (data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    html += '<li class="accordion-item"><div class="accordion-item-title"><p><img alt="' + data[i].category_name + '" src="' + data[i].category_icon + '">' + data[i].category_name + ' (' + data[i].dealers.length + ')</p></div><div class="accordion-item-content"><div class="accordion-item-content-dealer">';
                    //Loop for each dealer-row
                    // html += '<div class="dealer-row">';
                    for (var j = 0; j < data[i].dealers.length; j++) {
                        html += this.generateDealerItem(data[i].category_type, data[i].dealers[j].logo, data[i].dealers[j].name, data[i].dealers[j].email, data[i].dealers[j].address, data[i].dealers[j].tel, data[i].category_color, data[i].dealers[j].website, data[i].dealers[j].direction);
                        totalFoundItems++;
                    }
                    html += '</div></div></li>';
                }

            } else {
                html += '';
            }
            html += '</ul></div></div></div></div></div></div>';

            this.generateSummary(totalFoundItems, this.query.searchText);

            return html;
        }

        private generateSummary(total, location) {
            var html = '';
            html += '<strong>' + total + '</strong> ';
            html += $('.search-summary', this.jqueryElement).attr('data-dealers');

            if (location) {
                html += ' in <strong>' + location + '</strong>';
            }

            $('.search-summary', this.jqueryElement).html(html);
        }
        
        private registerEvents() {

            let self = this;

            var input = <HTMLInputElement>document.getElementById('search-dealer');

            var searchBox = new google.maps.places.SearchBox(input);
            searchBox.addListener('places_changed', function () {
                var places = searchBox.getPlaces();

                if (places && places.length > 0) {

                    self.query.latitude = places[0].geometry.location.lat();
                    self.query.longtitude = places[0].geometry.location.lng();
                } else {
                    self.query.latitude = self.query.longtitude = null;
                }
            });
            
            $('.use-current-location a', self.jqueryElement).on('click', function () {
                if ("geolocation" in navigator) {
                    navigator.geolocation.getCurrentPosition(function (position) {

                        self.query.latitude = position.coords.latitude;
                        self.query.longtitude = position.coords.longitude;
                        var geocoder = new google.maps.Geocoder();
                        var latlng = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
                        geocoder.geocode(<google.maps.GeocoderRequest>{ 'latLng': latlng }, function (results, status) {
                            if (status == google.maps.GeocoderStatus.OK) {
                                console.log(results)
                                if (results[1]) {
                                    //formatted address
                                    var address = results[0].formatted_address;

                                    $(input).val(address);
                                }
                            }
                        });

                    });
                } else {
                    console.log("Browser doesn't support geolocation!");
                }
            });

            $('.toggle-dealer-map', self.jqueryElement).on('click', function () {
                $(this).toggleClass('map-on');
                if ($(this).hasClass('map-on')) {
                    $(this).find('span').text($(this).data('text-hide'));
                } else {
                    $(this).find('span').text($(this).data('text-show'));
                }
                $('.map-wrapper').toggleClass('hide-map');
            });

            $('#btnSearchDealer', self.jqueryElement).on('click', function () {
                $(this).addClass('searching');
                self.loadDealers();
            });

            $(input).on('change', function (e) {
                // reset value
                self.query.latitude = self.query.longtitude = null;
            });
        }

        private registerControlEvents() {
            var self = this;
            $('.toggle-dealer-map', self.jqueryElement).on('click', function () {
                $(this).toggleClass('map-on');
                if ($(this).hasClass('map-on')) {
                    $(this).find('span').text($(this).data('text-hide'));
                } else {
                    $(this).find('span').text($(this).data('text-show'));
                }
                $('#netafim-dealer-map', self.params).toggleClass('show-dealer-map');
            });
        }

        private generateDealerItem(type, logo, name, email, address, tel, color, website, direction) {
            var html = '';
            html = '<div class="dealer-wrapper ' + color + '" data-dealer-type="' + type + '">';

            if (this.hasContent(logo)) {
                html += '<img class="dealer-logo" alt= "' + name + '" src= "' + logo + '" >';
            }

            if (this.hasContent(name)) {
                html += '<h5>' + name + ' </h5>';
            }

            if (this.hasContent(address)) {
                html += '<div class="dealer-address"><p>' + address + '</p></div>';
            }
            if (this.hasContent(tel)) {
                html += '<div class="dealer-phone"><a href="tel:' + tel + '">' + tel + '</a></div>';
            }
            html += '<div class="dealer-accessibility">';

            if (this.hasContent(email)) {
                html += '<a class="dealer-icon-chat" href="javascript:void(0);" data-open="popup-overlay" data-popup-id="dealer-locator-contact-form"><span>'+ this.params.contactFormLabel +'</span></a>';
            }
            if (this.hasContent(website)) {
                html += '<a class="dealer-icon-website" href="' + website + '"><span>' + this.params.websiteLabel +'</span></a>';
            }
            if (this.hasContent(direction)) {
                html += '<a class="dealer-icon-direction" href="' + direction + '"><span>' + this.params.directionsLabel +'</span></a>';
            }
            html += '</div></div>';

            return html;
        }

        private infoBox(type, logo, name, email, address, tel, color) {
            var html = '<div class="dealer-info-window ' + color + '" data-dealer-type="' + type + '">';

            if (this.hasContent(logo)) {
                html += '<img alt="' + name + '" src="' + logo + '">';
            }

            if (this.hasContent(name)) {
                html += '<h5>' + name + '</h5>';
            }
            
            html += '<p>';
            if (this.hasContent(address)) {
                html += address;
            }
            if (this.hasContent(tel)) {
                html += '<br>' + tel;
            }
            html += '</p>';

            if (this.hasContent(email)) {
                html += '<div class="dealer-accessibility"><a href="javascript:void(0);" data-open="popup-overlay" data-popup-id="dealer-locator-contact-form" class="dealer-icon-chat">' + this.params.contactFormLabel +'</a></div>';
            }
            html += '</div>';

            return html;
        }

        private hasContent(content) {
            return content != '' && content != null && content != undefined
        }

        private registerContactForm(parentEle) {
            var self = this;
            // Open overlay
            $(parentEle + ' a[data-open="popup-overlay"]', self.jqueryElement).on('click', function (e) {
                $('body').addClass('no-scroll');

                var popupId = $(this).attr("data-popup-id");
                $('.popup-overlay[data-popup-id="' + popupId + '"]').addClass('show-popup');
                e.preventDefault();
            });
        }
    }
}