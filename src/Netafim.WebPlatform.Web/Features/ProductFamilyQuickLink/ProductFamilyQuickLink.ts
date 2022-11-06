module Netafim.Web {
    export class ProductFamilyQuickLink {
        constructor(jqueryElement: any) {
            let self = this;

            $(".FormSelection select", jqueryElement).on("change", function () {
                self.goToFamilyDetailsPage(jqueryElement);
            });
        }

        private goToFamilyDetailsPage(jqueryElement) {
            var link = $(".FormSelection select", jqueryElement).val();
            if (link === "")
                return;

            window.location.href = link;
        }
    }
}