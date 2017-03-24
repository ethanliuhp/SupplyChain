// Ionic Starter App

// angular.module is a global place for creating, registering and retrieving Angular modules
// 'starter' is the name of this angular module example (also set in a <body> attribute in index.html)
// the 2nd parameter is an array of 'requires'
// 'starter.services' is found in services.js
// 'starter.controllers' is found in controllers.js
angular.module('starter', ['ionic', 'starter.controllers', 'starter.services'])

.run(function($ionicPlatform,$ionicPlatform) {
  $ionicPlatform.ready(function() {
    // Hide the accessory bar by default (remove this to show the accessory bar above the keyboard
    // for form inputs)
    if (window.cordova && window.cordova.plugins && window.cordova.plugins.Keyboard) {
      cordova.plugins.Keyboard.hideKeyboardAccessoryBar(true);
      cordova.plugins.Keyboard.disableScroll(true);

    }

    if (window.StatusBar) {
      // org.apache.cordova.statusbar required
      StatusBar.styleDefault();
    }
  });
})

.config(function($stateProvider, $urlRouterProvider,$ionicConfigProvider) {

  // Ionic uses AngularUI Router which uses the concept of states
  // Learn more here: https://github.com/angular-ui/ui-router
  // Set up the various states which the app can be in.
  // Each state's controller can be found in controllers.js

  $ionicConfigProvider.platform.android.navBar.alignTitle('center');
  // $ionicConfigProvider.platform.ios.tabs.style('standard'); 
  // $ionicConfigProvider.platform.ios.tabs.position('bottom');
  $ionicConfigProvider.platform.android.tabs.style('standard');
  $ionicConfigProvider.platform.android.tabs.position('standard');
  $stateProvider

  // setup an abstract state for the tabs directive
  .state('tab', {
    url: '/tab',
    abstract: true,
    templateUrl: 'templates/tabs.html'
  })
  // Each tab has its own nav history stack:

  .state("tab.index",{
    url:"/index",
    views:{
      "tab-index":{
        templateUrl:"templates/fundManagement/tab-index.html",
        controller:"IndexCtrl"
      }
    }
  })
  .state("tab.index-businessincome",{
    url:"/index/businessincome",
    views:{
      "tab-index":{
        templateUrl:"templates/fundManagement/index-businessincome.html",
        controller:"BusinessincomeCtrl"
      }
    }
  })
  .state("tab.index-stockmomey",{
    url:"/index/stockmomey",
    views:{
      "tab-index":{
        templateUrl:"templates/fundManagement/index-stockmomey.html",
        controller:"StockmomeyCtrl"
      }
    }
  })
  .state("tab.index-receivables",{
    url:"/index/receivables",
    views:{
      "tab-index":{
        templateUrl:"templates/fundManagement/index-receivables.html",
        controller:"ReceivablesCtrl"
      }
    }
  })
  .state("tab.index-accountsreceivable",{
    url:"/index/accountsreceivable",
    views:{
      "tab-index":{
        templateUrl:"templates/fundManagement/index-accountsreceivable.html",
        controller:"AccountsReceivableCtrl"
      }
    }
  })
  .state("tab.index-arrears",{
    url:"/index/arrears",
    views:{
      "tab-index":{
        templateUrl:"templates/fundManagement/index-arrears.html",
        controller:"ArrearsCtrl"
      }
    }
  })
  .state("tab.index-occupy",{
    url:"/index/occupy",
    views:{
      "tab-index":{
        templateUrl:"templates/fundManagement/index-occupy.html",
        controller:"OccupyCtrl"
      }
    }
  })
  .state("tab.index-cashrate",{
    url:"/index/cashrate",
    views:{
      "tab-index":{
        templateUrl:"templates/fundManagement/index-cashrate.html",
        controller:"CashrateCtrl"
      }
    }
  })
  .state("tab.index-margin",{
    url:"/index/margin",
    views:{
      "tab-index":{
        templateUrl:"templates/fundManagement/index-margin.html",
        controller:"MarginCtrl"
      }
    }
  })
  .state("tab.businessincome-detail",{
    url:"/index/businessincome/detail/:id",
    views:{
      "tab-index":{
        templateUrl:"templates/fundManagement/businessincome-detail.html",
        controller:"BusinessincomeDetailCtrl"
      }
    }
  })
  .state("tab.cashrate-detail",{
    url:"/index/cashrate/detail/:id",
    views:{
      "tab-index":{
        templateUrl:"templates/fundManagement/cashrate-detail.html",
        controller:"CashrateDetailCtrl"
      }
    }
  })
  .state("tab.receivables-detail",{
    url:"/index/receivables/detail/:id",
    views:{
      "tab-index":{
        templateUrl:"templates/fundManagement/receivables-detail.html",
        controller:"ReceivablesDetailCtrl"
      }
    }
  })
  .state("tab.occupy-detail",{
    url:"/index/occupy/detail/:id",
    views:{
      "tab-index":{
        templateUrl:"templates/fundManagement/occupy-detail.html",
        controller:"OccupyDetailCtrl"
      }
    }
  })
  .state("tab.accountsreceivable-detail",{
    url:"/index/accountsreceivable/detail/:id",
    views:{
      "tab-index":{
        templateUrl:"templates/fundManagement/accountsreceivable-detail.html",
        controller:"AccountsreceivableDetailCtrl"
      }
    }
  })
  .state("tab.arrears-detail",{
    url:"/index/arrears/detail/:id",
    views:{
      "tab-index":{
        templateUrl:"templates/fundManagement/arrears-detail.html",
        controller:"ArrearsDetailCtrl"
      }
    }
  })

  // if none of the above states are matched, use this as the fallback
  $urlRouterProvider.otherwise('/tab/index');

});
