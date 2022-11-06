module Netafim.Web {
    interface ISuccessStoryFilter {
        blockId: number,
        searchUrl: string,
        repeatedCriteriaTranslated: string,
    }

    interface IDataFilter {
        BlockId: number,
        BigProject: boolean,
        CropId: number,
        Country: string,
        CurrentPage: number,
        HasHashData: boolean,
    }
    
    var hash: Netafim.Web.HashService;

    export class SuccessStoryOverview {
        params: ISuccessStoryFilter;
        dataFilter: IDataFilter;
        jqueryElements: any;
        constructor(jqueryElement: any, parameter: ISuccessStoryFilter) {
            let self = this;
            this.params = parameter;
            this.jqueryElements = jqueryElement;
            hash = new HashService();

            this.dataFilter = {
                CropId: 0,
                Country: '',
                BigProject: false,
                BlockId: this.params.blockId,
                CurrentPage: 1,
                HasHashData: false
            };

            self.doInitFilter();

            //Live search
            $('.live-search-list li', jqueryElement).each(function () {
                $(this).attr('data-search-term', $(this).text().toLowerCase());
            });

            $('.live-search-box input', jqueryElement).on('focusin', function () {
                $('.live-search-box input[type="text"]').each(function () {
                    if ($(this).next('.live-search-list').hasClass('show-search-list')) {
                        $(this).next('.show-search-list').removeClass('show-search-list');
                    }
                });
                $(this).parent().find('.live-search-list').addClass('show-search-list');
            });

            $('.live-search-list li', jqueryElement).on('click', function () {
                var selectedText = $(this).text();
                var selectedList = $(this).parent();
                $(this).parent().siblings('.dropdown-textbox').attr('value', selectedText);
                $(this).parent().removeClass('show-search-list');
                //Start reloading the content below, this will be dealt by BE dev :)
                //Code here....
                if (selectedList.hasClass('crop-list')) {
                    self.dataFilter.CropId = parseInt($(this).data("value"));
                } else if (selectedList.hasClass('country-list')) {
                    self.dataFilter.Country = $(this).data("value");
                }
                
                if ($('#open-field-crop', jqueryElement).is(":checked")) {
                    self.dataFilter.BigProject = true;
                } else {
                    self.dataFilter.BigProject = false;
                }

                self.doUpdateFilter();
            });

            $('#open-field-crop', jqueryElement).change(function() {
                if (this.checked) {
                    self.dataFilter.BigProject = true;
                } else {
                    self.dataFilter.BigProject = false;
                }
                self.doUpdateFilter();
            });

            $(document).on('click', function (e) {
                if (!$(e.target).is('.live-search-item') && !$(e.target).is('.dropdown-textbox')) {
                    $('.show-search-list').removeClass('show-search-list');
                }
            });

            $('.live-search-box input.dropdown-textbox', jqueryElement).on('keyup', function () {
                var searchTerm = $(this).val().toLowerCase();
                $('.live-search-list li').each(function () {
                    if ($(this).filter('[data-search-term *= ' + searchTerm + ']').length > 0 || searchTerm.length < 1) {
                        $(this).show();
                    } else {
                        $(this).hide();
                    }
                });
            });

            //success-stories-overview .loadmore-btn
            $('.load-more-row .loadmore-btn', jqueryElement).on('click', function () {
                $(this).addClass('searching');
                self.dataFilter.CurrentPage = self.dataFilter.CurrentPage + 1;
                self.doLoadMore();
            });
        }
        
        private doInitFilter() {
            var hashObject = hash.getHash(this.params.blockId);
            if (hashObject != null) {
                this.dataFilter.HasHashData = true;
                this.dataFilter.CropId = parseInt(hashObject["crop"]);
                this.dataFilter.Country = hashObject["country"];
                this.dataFilter.BigProject = hashObject["bigProject"] === "true";
                this.dataFilter.BlockId = parseInt(hashObject["contentId"]);
                this.dataFilter.CurrentPage = parseInt(hashObject["currentPage"]);

                this.doPrefillControl();
            }

            this.doAjax(data => { this.replaceCallback(false, data); });
        }

        private doUpdateFilter() {
            this.dataFilter.CurrentPage = 1;
            this.dataFilter.HasHashData = false;
            this.doAjax(data => { this.replaceCallback(true, data); });
        }

        private doLoadMore() {
            this.dataFilter.HasHashData = false;
            this.doAjax(data => { this.appendCallback(data); });
        }

        private replaceCallback(isReplaceData, data) {
            $(".row .story-grid", this.jqueryElements).html(data.html);
            $(".result-counter span.result-number", this.jqueryElements).text(data.totalsResult);
            var repeatedText = "";
            if ((this.dataFilter.Country !== "" || this.dataFilter.CropId > 0) && parseInt(data.totalsResult) > 0) {
                var country = this.dataFilter.Country !== "" ? this.getCountryNameByCode(this.dataFilter.Country) : "";
                var crop = this.dataFilter.CropId > 0 ? this.getCropTextById(this.dataFilter.CropId) : "";
                var hasAnd = country !== "" && crop !== "" ? " " +this.params.repeatedCriteriaTranslated+ " " : "";
                repeatedText = "for " + country + hasAnd + crop;
            }
            $(".result-counter span.repeated-criteria", this.jqueryElements).text(repeatedText);
            if (isReplaceData) {
                this.bindHashToUrl();
            }
            this.setLoadMoreState(this.dataFilter.CurrentPage, data.totalPage);
        }

        private appendCallback(data) {
            $(data.html).appendTo($(".row .story-grid", this.jqueryElements));
            this.bindHashToUrl();
            this.setLoadMoreState(this.dataFilter.CurrentPage, data.totalPage);

            $('.load-more-row .loadmore-btn', this.jqueryElements).removeClass('searching');
        }

        private doAjax(successCallback) {
            $.ajax({
                url: '/' + this.params.searchUrl,
                data: this.dataFilter,
                dataType: 'json',
                method: 'POST',
                cache: false,
                success: function (data) {
                    successCallback(data);
                    console.log('success: ' + data);
                },
                error: function (jqXHR) {
                    console.log('error: ' + jqXHR);
                },
                complete: function (jqXHR) {
                    console.log('complete: ' + jqXHR);
                }
            });
        }

        private bindHashToUrl() {
            var obj = {
                crop: this.dataFilter.CropId,
                country: this.dataFilter.Country,
                bigProject: this.dataFilter.BigProject,
                currentPage: this.dataFilter.CurrentPage
            };
            hash.setHash(this.params.blockId, obj);
        }

        private setLoadMoreState(currentPage, totalPage) {
            var loadMore = $('.load-more-row .loadmore-btn', this.jqueryElements);
            if (currentPage === totalPage || totalPage === 0) {
                loadMore.hide();
            } else {
                loadMore.show();
            }
        }

        private doPrefillControl() {
            $("#open-field-crop", this.jqueryElements).prop("checked", this.dataFilter.BigProject);
            if (this.dataFilter.CropId > 0) {
                $(".crop-list", this.jqueryElements).siblings(".dropdown-textbox").attr("value", this.getCropTextById(this.dataFilter.CropId));
            }

            if (this.dataFilter.Country !== "") {
                $(".country-list", this.jqueryElements).siblings(".dropdown-textbox").attr("value", this.getCountryNameByCode(this.dataFilter.Country));    
            }
        }


        private getCountryNameByCode(countryCode) {
            var getCountry = "";
            $(".country-list li.live-search-item", this.jqueryElements).each(function() {
                if ($(this).data('value') === countryCode) {
                    getCountry = $(this).text();
                    return false;
                }
            });

            return getCountry;
        }

        private getCropTextById(cropId) {
            var cropText = "";
            $(".crop-list li.live-search-item", this.jqueryElements).each(function () {
                if ($(this).data('value') === cropId) {
                    cropText = $(this).text();
                    return false;
                }
            });

            return cropText;
        }
    }
}