angular.module("umbraco.services").factory("cookieService", function ($http) {
    return {
        setCookie: function () {
            return $http.post(`/umbraco/backoffice/2fa/Cookie/SetCookie`);
        }
    };
});