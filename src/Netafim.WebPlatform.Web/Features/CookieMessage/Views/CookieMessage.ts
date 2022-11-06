module Netafim.Web {

    export class CookieMessage {
        constructor(jqueryElement: any) {
            if (document.cookie.indexOf("netafim_visited=") >= 0) {
                $('body').removeClass('has-cookie');
            } else {
                $('body').addClass('has-cookie');
            }

            $(jqueryElement).find('.cookie .accept-cookie').on('click', function () {
                //Set netafim_visited cookie
                var expiry = new Date();
                expiry.setFullYear(expiry.getFullYear() + 1);
                document.cookie = "netafim_visited=yes; expires=" + expiry.toUTCString();
                $('body').removeClass('has-cookie');
            });
        }
    }
}