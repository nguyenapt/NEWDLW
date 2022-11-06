module Netafim.Web {
    export class CustomForm {
        constructor(jqueryElement: any) {
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