module Netafim.Web {
    export class SidebarNavigation {
        constructor(jqueryElement: any) {
            $('.sidebar-nav-menu').on('click', function () {
                $('.sidebar-navigation').toggleClass('show-nav');
            });           
        }
    }
}