angular.module("umbraco.services").factory("adminUserService", function($http) {
    return {
        deleteTwoFactor: function(users) {
            return $http.post(`/umbraco/backoffice/2fa/AdminUser/DeleteTwoFactor?input=${JSON.stringify(users)}`);
        },
        getTwoFactor: function(page) {
            return $http.get(`/umbraco/backoffice/2fa/AdminUser/GetAllInfo?page=${page}`);
        },
        pages: function() {
            return $http.get(`/umbraco/backoffice/2fa/AdminUser/Pages`);
        }
    };
});
