(function () {
    'use strict';

    angular.module('folderBrowserApp', [])
        .controller('folderController', function ($http) {
            var vm = this;

            
            vm.filesAmounts = {
                filesUnderTen: 0,
                filesTenToFifty: 0,
                filesAboveHundred: 0
            };
            vm.currentPath = '';
            vm.files = [];

            $http.get('/api/folders')
                .then(
                //Success
                function (responce) {
                    vm.filesAmounts.filesAboveHundred = responce.data.FilesAboveHundred;
                    vm.filesAmounts.filesTenToFifty = responce.data.FilesTenToFifty;
                    vm.filesAmounts.filesUnderTen = responce.data.FilesUnderTen;
                    vm.currentPath = responce.data.CurrentFolder;
                    angular.copy(responce.data.Files, vm.files);
                },
                //Error
                function (error) {
                    console.log('something gone wrong' + error);
                }
                )

            vm.GoTo = function (folderName) {
                $http.get('/api/folders' + '?newFolder=' + folderName)
                    .then(
                    //Success
                    function (responce) {
                        vm.filesAmounts.filesAboveHundred = responce.data.FilesAboveHundred;
                        vm.filesAmounts.filesTenToFifty = responce.data.FilesTenToFifty;
                        vm.filesAmounts.filesUnderTen = responce.data.FilesUnderTen;
                        vm.currentPath = responce.data.CurrentFolder;
                        angular.copy(responce.data.Files, vm.files);
                    },
                    //Error
                    function (error) {
                        console.error(error);
                    }
                    )
            }
        });


    
})();