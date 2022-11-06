module Netafim.Web {

    interface IJobFilter {
        blockId: number,
        searchUrl: string,
    }

    export class JobFilter {
        getLocation(jqueryElement: any): string {
            var seletedLocation = $('.FormSelection select.location', jqueryElement).val();
            if (seletedLocation) return seletedLocation;
            return "";
        }
        getDepartment(jqueryElement): string {
            var selectedDepartment = $('.FormSelection select.department', jqueryElement).val();
            if (selectedDepartment) return selectedDepartment;
            return "";
        }

        params: IJobFilter;
        constructor(jqueryElement: any, parameter: IJobFilter) {
            this.params = parameter;
            let self = this;

            //Job over-view
            $('.FormSelection select', jqueryElement).on('change', function () {
                self.searchJobs(jqueryElement);
            });

            this.searchJobs(jqueryElement);
        }

        private searchJobs(jqueryElement) {
            let data = {
                Department: this.getDepartment(jqueryElement),
                Location: this.getLocation(jqueryElement),
                BlockId: this.params.blockId                
            };

            $.ajax({
                url: '/' + this.params.searchUrl,
                data: data,
                dataType: 'html',
                method: 'POST',
                cache: false,
                success: function (res) {
                    $(".data-result", jqueryElement).html(res);
                },
                error: function (jqXHR) {

                }
            });

        }     
    }
}