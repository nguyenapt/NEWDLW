module Netafim.Web {
    export class FormContainerBlock {
        constructor(jqueryElement: any) {

            //Fixing Episerver FromChoice
            $('.netafim-form .FormChoice label', jqueryElement).each(function () {
                var newHtml = $(this).find('.FormChoice__Input--Checkbox').clone().wrap('<div/>').parent().html() + '<span>' + $(this).text() + '</span>';
                $(this).html(newHtml);
            });

            $(".netafim-form .EPiServerForms .FormSubmitButton", jqueryElement).on("click", function () {
                if ($(".netafim-form .EPiServerForms .Form__Success__Message").length > 0) {
                    $(".extra-information", jqueryElement).hide();
                }
            });
            
            $("[context-aware]", jqueryElement).each(function () {
                var $context = $(this);
                var hiddenMetadata = '',
                    metadata = $context
                        .find('[public-identity-metadata] > input[public-identity-value]')
                        .map(function () {
                            var $prop = $(this);

                            hiddenMetadata += '<input name=\"__contextaware_\" value=\"' + $prop.attr('public-identity-value') + '\"' +
                                'type =\"hidden\" class=\"Form__Element FormHidden FormHideInSummarized Form__Element--Filled\">';
                        });

                $context
                    .find("form.EPiServerForms")
                    .each(function () {
                        $(this).find(".Form__MainBody section#__field_").append(hiddenMetadata);
                    });
            });
        }
    }
}