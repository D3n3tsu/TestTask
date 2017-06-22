(function () {
    'use strict';

    angular.module('folderBrowserApp', [])
        .controller('folderController', function ($http) {
            var vm = this;

            vm.busy = true;
            
            vm.filesAmounts = {
                filesUnderTen: 0,
                filesTenToFifty: 0,
                filesAboveHundred: 0
            };
            vm.currentPath = '';
            vm.files = [];

            

            vm.GoTo = function () {
                vm.busy = true;
                var url = '/api/folders';
                //if method is used at start of the program use simple api call to get folders
                //else call api with query string
                if (arguments[0] != undefined)
                    url += '?newFolder=' + arguments[0]; 
                $http.get(url)
                    .then(
                    //Success
                    function (responce) {
                        vm.filesAmounts.filesAboveHundred = responce.data.FilesAboveHundred;
                        vm.filesAmounts.filesTenToFifty = responce.data.FilesTenToFifty;
                        vm.filesAmounts.filesUnderTen = responce.data.FilesUnderTen;
                        vm.currentPath = responce.data.CurrentFolder;
                        angular.copy(responce.data.Files, vm.files);
                        vm.busy = false;
                    },
                    //Error
                    function (error) {
                        console.error(error);
                        vm.busy = false;
                    }
                    )
            }

            vm.GoTo();
        });


    
})();