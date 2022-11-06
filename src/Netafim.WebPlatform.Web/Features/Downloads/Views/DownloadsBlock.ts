var dataLayer;
module Netafim.Web {
    export class DownloadsBlock {        
        constructor(jqueryElement: any) {            
            $('.download-item', $(jqueryElement).parent()).each(function () {
                $(this).click(function () {
                    var fileName = $(this).attr("data-title");
                    if (dataLayer) {
                        dataLayer.push({
                            'event': 'fileDownload',
                            'downloadName': fileName
                        });
                    }                    
                });
            });         
        }
    }
}