module Netafim.Web {
    export class HashService {
        constructor() {
        }

        setHash(contentId, hashObject) {
            var objectHashs = this.listObjectsFromHashUrl();
            if (objectHashs.length === 0) {
                window.location.hash = this.parseObjectHashUrl(contentId, hashObject);
            } else {
                var hash = "";
                for (var i = 0; i < objectHashs.length; i++) {
                    // update exsited object
                    if (objectHashs[i].key !== contentId) {
                        hash += "&" + this.parseObjectHashUrl(objectHashs[i].key, objectHashs[i].data);
                    }
                }
                hash = this.parseObjectHashUrl(contentId, hashObject) + hash;
                window.location.hash = hash;
            }
        }

        getHash(contentId) {
            var objectHashs = this.listObjectsFromHashUrl();
            if (objectHashs.length === 0) return null;
            for (var i = 0; i < objectHashs.length; i++) {
                if (objectHashs[i].key === contentId)
                    return objectHashs[i].data;
            }
            return null;
        }

        private listObjectsFromHashUrl() {
            var res = [];
            var hashUrl = window.location.hash;
            if (!hashUrl) return res;

            if (hashUrl.match("^#")) {
                hashUrl = hashUrl.substring(1);
            }

            var hashSplit = hashUrl.split('contentId');
            if (hashSplit.length === 0) return res;

            for (var i = 0; i < hashSplit.length; i++) {
                var hashVal = hashSplit[i];
                if (hashVal === "") continue;

                hashVal = "contentId" + hashVal;
                var hashObject = this.parseHashUrlToObject(hashVal);

                res.push({
                    key: parseInt(hashObject["contentId"]),
                    data: hashObject
                });
            }
            return res;
        }

        private parseObjectHashUrl(contentId, hashObject) {
            var hash = "";
            for (var key in hashObject) {
                if (hashObject.hasOwnProperty(key)) {
                    if (hash !== "") {
                        hash += "&";
                    }
                    hash += key + "=" + encodeURIComponent(hashObject[key]);
                }
            }
            if (!hashObject.hasOwnProperty("contentId"))
                hash = "contentId=" + contentId + "&" + hash;
            return hash;
        }

        private parseHashUrlToObject(query) {
            if (query) {
                var params = {}, e;
                var re = /([^&=]+)=?([^&]*)/g;
                if (query.substr(0, 1) === '?') {
                    query = query.substr(1);
                }
                while (e = re.exec(query)) {
                    var k = decodeURIComponent(e[1].replace(/\+/g, ' '));
                    var v = decodeURIComponent(e[2].replace(/\+/g, ' '));
                    if (params[k] !== undefined) {
                        if (!$.isArray(params[k])) {
                            params[k] = [params[k]];
                        }
                        params[k].push(v);
                    } else {
                        params[k] = v;
                    }
                }
                return params;
            }
            return null;
        }
    }
}