module Netafim.Web {
    interface IInput {
        searchOfficesUrl: string;
    }

    export class OfficeLocator {

        params: IInput;
        prevInfoWindow: google.maps.InfoWindow;
        private ICON_BASE_URL: string = '/Content/images/';
        private notNeedRenderForMobile: boolean;
        private jqueryElement: any;

        constructor(jqueryElement: any, parm: IInput) {

            this.params = parm;

            this.jqueryElement = jqueryElement;

            this.notNeedRenderForMobile = true;

            this.registerEvents();

            this.loadOffices({});
        }

        private initMap(lat?: number, lng?: number) {

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

            lat = lat || 59.325;
            lng = lng || 18.070;

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


            let map = new google.maps.Map(document.getElementById("netafim-map"), opt);

            return map;
        }

        private loadOffices(query) {
            var self = this;

            $.ajax({
                type: "POST",
                url: this.params.searchOfficesUrl,
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(query),
                success: function (data) {

                    self.populateOffices(data, query.Latitude, query.Longtitude);
                }
            });
        }

        private populateOffices(data: any, lat?: number, lng?: number) {

            var offices = $.parseJSON(data);

            let map = this.initMap(lat, lng);

            let bounds = new google.maps.LatLngBounds();

            $('.netafim-map-result').html('');
            //Adding Markers to the existing maps
            for (var i = 0, length = offices.length; i < length; i++) {
                var office = offices[i];

                this.createMaker(office, map, bounds);

                if (!this.notNeedRenderForMobile) // Add to mobile map
                    $('.netafim-map-result').append(this.resultBox(office.officeName, office.address, office.phone, office.fax, office.email, office.website, office.direction));                
            }

            if (this.notNeedRenderForMobile)
                this.notNeedRenderForMobile = false;

            if (offices.length <= 0) // update fitbound
            {
                  // Add no result to the result for mobile mode
                $('.netafim-map-result').append($('.no-result-wrapper', this.jqueryElement).html());

                var position = new google.maps.LatLng(lat, lng);
                bounds.extend(position);
            }

            map.fitBounds(bounds);

            if (offices.length <= 1) {
                map.setZoom(5);
            }

            this.setControlState(lat && lng);
        }

        private createMaker(office, map, bounds) {

            var position = new google.maps.LatLng(office.latitude, office.longtitude);

            var options: google.maps.MarkerOptions = {
                position: position,
                map: map,
                icon: this.ICON_BASE_URL + 'icon-pin.png',
                animation: google.maps.Animation.DROP,
                title: office.title
            };

            let infoWindow = new google.maps.InfoWindow();
            var marker = new google.maps.Marker(options);

            bounds.extend(position);

            let content = this.infoBox(office.officeName, office.address, office.phone, office.fax, office.email, office.website, office.direction);
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
                        this.prevInfoWindow = infoWindow;
                        google.maps.event.addListener(map,
                            'click',
                            () => {
                                if (infoWindow) infoWindow.close();
                            });
                    };
                })(marker, content, infoWindow));
        }

        private registerEvents() {

            let self = this;

            var input = <HTMLInputElement>document.getElementById('search-location');

            var searchBox = new google.maps.places.SearchBox(input);
            searchBox.addListener('places_changed', function () {
                var places = searchBox.getPlaces();

                if (places && places.length > 0) {
                    var latlng = { Latitude: places[0].geometry.location.lat(), Longtitude: places[0].geometry.location.lng() };

                    self.parseCountryAndLoadOffice(latlng);

                    self.gtmTracking(places[0].formatted_address);
                }
            });

            $('#btn-clear-search').on('click', function () {
                self.notNeedRenderForMobile = true;
                self.loadOffices({});
            });
        }

        private setControlState(enableClearButton) {

            if (enableClearButton) {
                $("#btn-clear-search").show();
            } else {
                $("#btn-clear-search").hide();
                $("#search-location").val('');
            }
        }
        
        private icon_email: string = this.ICON_BASE_URL + 'icon-mail.png';
        private icon_website: string = this.ICON_BASE_URL + 'icon-globe.png';
        private icon_direction: string = this.ICON_BASE_URL + 'icon-directions.png';
        
        private infoBox(title, address, tel, fax, email, website, direction) {

            var html = '<div class="map-popup-container">';

            if (this.hasContent(title)) {
                html += '<h3>' + title + '</h3><div class="map-col">';
            }

            if (this.hasContent(address)) {
                html += '<p>' + address + '</p>';
            }

            if (this.hasContent(tel)) {
                html += '<p><span class="blue-text">Tel</span><a href="tel:' + tel + '">' + tel + '</a><br>';
            }
            if (this.hasContent(fax)) {
                html += '<p><span class="blue-text">Fax</span>' + fax + '</p>';
            }
            html += '</div><div class="map-col">';
            if (this.hasContent(email)) {
                html += '<p><a href="mailto:' + email + '"><img alt="Email" src="' + this.icon_email + '">' + email + '</a></p>';
            }
            if (this.hasContent(website)) {
                html += '<p><a href="' + website + '"><img alt="Website" src="' + this.icon_website + '">' + website + '</a></p>';
            }
            if (this.hasContent(direction)) {
                html += '<p><a href="' + direction + '" target="_blank"><img alt="Website" src="' + this.icon_direction + '">Direction</a></p>';
            }
            html += '</div></div>';
            return html;
        }

        private resultBox(title, address, tel, fax, email, website, direction) {
            var html = '<div class="address-box">';

            if (this.hasContent(title)) {
                html += '<h3>' + title + '</h3>';
            }

            html += '<div class="address-content">';

            if (this.hasContent(address)) {
                html += '<p>' + address + '</p>';
            }

            if (this.hasContent(tel)) {
                html += '<p><span class="blue-text">Tel</span><a href="tel:' + tel + '">' + tel + '</a></p>';
            }
            if (this.hasContent(fax)) {
                html += '<p><span class="blue-text">Fax</span>' + fax + '</p>';
            }
            if (this.hasContent(email)) {
                html += '<p><a href="mailto:' + email + '"><img alt="Email" src="' + this.icon_email + '">' + email + '</a></p>';
            }
            if (this.hasContent(website)) {
                html += '<p><a href="' + website + '"><img alt="Website" src="' + this.icon_website + '">' + website + '</a></p>';
            }
            if (this.hasContent(direction)) {
                html += '<p><a href="' + direction + '" target="_blank"><img alt="Website" src="' + this.icon_direction + '">Direction</a></p>';
            }
            html += '</div></div>';
            return html;
        }
        
        private hasContent(content) {
            return content != '' && content != null && content != undefined
        }

        private gtmTracking(query) {
            if (dataLayer) {
                dataLayer.push({
                    'event': 'locationSearch',
                    'searchQuery': query
                });
            } 
        }

        private parseCountryAndLoadOffice(latlng) {

            var query = new google.maps.LatLng(latlng.Latitude, latlng.Longtitude);
            var self = this;

            new google.maps.Geocoder().geocode(<google.maps.GeocoderRequest>{ 'latLng': query }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    if (results[1]) {
                        var countryCode = null;
                        var c, lc, component;
                        for (var r = 0, rl = results.length; r < rl; r += 1) {
                            var result = results[r];

                            if (!countryCode && result.types[0] === 'country') {
                                countryCode = result.address_components[0].short_name;
                                break;
                            }                            
                        }

                        if (countryCode) {
                            var officeQuery = { Country: countryCode, Latitude: latlng.Latitude, Longtitude: latlng.Longtitude };

                            self.loadOffices(officeQuery);
                        }
                    }
                }
            });
        }
    }
}