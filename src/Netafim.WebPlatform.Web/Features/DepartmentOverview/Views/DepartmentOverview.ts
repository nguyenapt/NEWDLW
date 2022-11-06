module Netafim.Web {

    interface IDepartmentInput {
        jobFilterId: string
    }

    export class DepartmentOverview {
        constructor(jqueryElement: any, param: IDepartmentInput) {

            $('.btn-filter-department', jqueryElement).on('click', function () {

                var self = this;

                if (param != null && param.jobFilterId != null && param.jobFilterId != '' && param.jobFilterId != undefined) {

                    // find all job filter component in this page
                    $("#" + param.jobFilterId).each(function (i, element) {

                        var department = $(self).attr('data-department');
                        // Pre-fill the department selection
                        var departmentSelection = $("select.department", element);
                        departmentSelection.val(department);

                        // reset the location
                        $("select.location", element).val('');

                        // Trigger the event.
                        departmentSelection.trigger('change');
                    });
                }

            })
        }
    }
}