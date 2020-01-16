angular.module("umbraco.services").factory("deviceService", function ($http) {
    return {
        getTwoFactorModels: function () {
            return $http.get(`/umbraco/backoffice/2fa/Device/GetTwoFactorModels`);
        },
        deleteDevices: function (devices) {
            return $http.post(`/umbraco/backoffice/2fa/Device/DeleteDevices?input=${JSON.stringify(devices)}`);
        },
        disableMail: function () {
            return $http.post(`/umbraco/backoffice/2fa/Device/DisableMail`);
        }
    };
});