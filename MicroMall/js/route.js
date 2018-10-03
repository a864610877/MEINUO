angular.module('router', ["ui.router"]).
run(function($rootScope, $location) {
	$rootScope.$on('$stateChangeStart', function() { //页面打开前
		$(".ssd_iiuu_xds").addClass("show")
	})
	$rootScope.$on('$stateChangeSuccess', function() { //页面打开成功
		$(".ssd_iiuu_xds").removeClass("show")
		var kmn_s = ["/forget_password", "/login", '/login2', '/register'];
		$(".sd_uyt_fotter").removeClass("box")
		try {
			plus.navigator.setStatusBarBackground("#EE6448"); //设置状态栏颜色
		} catch(e) {

		}
		for(var i = 0; i < kmn_s.length; i++) {
			if($location.path() == kmn_s[i]) {
				return
			}
		}
		try {
			plus.navigator.setStatusBarBackground("#DF1222"); //设置状态栏颜色
		} catch(e) {

		}

		$(".sd_uyt_fotter").addClass("box")
	})
}).
config(['$stateProvider', '$urlRouterProvider', function($stateProvider, $urlRouterProvider) {
	$urlRouterProvider.when('order-list', '/order-list') // 订单列表
		.otherwise('/shouye'); //登录    默认项
	$stateProvider.state('register', {
		url: '/register', //登录
		templateUrl: 'register.html',
		controller: 'register'
	}).state('login', { //注册
		url: '/login',
		templateUrl: 'login.html',
		controller: 'login'
	}).state('login2', { //注册下一步
		url: '/login2',
		templateUrl: 'login2.html',
		controller: 'login'
	}).state('forget_password', { //找回密码
		url: '/forget_password',
		templateUrl: 'forget_password.html',
		controller: 'forget_password'
	}).state('shouye', { //首页
		url: '/shouye',
		templateUrl: 'shouye.html',
		controller: 'shouye'
	}).state('discover', { //发现
		url: '/discover',
		templateUrl: 'discover.html',
		controller: 'discover'
	}).state('sound_adapter', { //发现
		url: '/sound_adapter',
		templateUrl: 'sound_adapter.html',
		controller: 'sound_adapter'
	}).state('information', { //消息
		url: '/information',
		templateUrl: 'information.html',
		controller: 'information'
	}).state('online_loans', { //在线贷款
		url: '/online_loans',
		templateUrl: 'online_loans.html',
		controller: 'online_loans'
	}).state('remind', { //提醒
		url: '/remind',
		templateUrl: 'remind.html',
		controller: 'remind'
	}).state('course_learning', { //课程学习
		url: '/course_learning',
		templateUrl: 'course_learning.html',
		controller: 'course_learning'
	}).state('teaching_details', { //课程详情
		url: '/teaching_details',
		templateUrl: 'teaching_details.html',
		controller: 'teaching_details'
	}).state('BBS_message', { //论坛消息
		url: '/BBS_message',
		templateUrl: 'BBS_message.html',
		controller: 'BBS_message'
		
	}).state('mine', { //我的
		url: '/mine',
		templateUrl: 'mine.html',
		controller: 'mine'
	}).state('learning_centre', { //学习豆
		url: '/learning_centre',
		templateUrl: 'learning_centre.html',
		controller: 'mine'
	}).state('my_favorite', { //学习豆
		url: '/my_favorite',
		templateUrl: 'my_favorite.html',
		controller: 'my_favorite'
	}).state('personal_setting', { //个人设置
		url: '/personal_setting',
		templateUrl: 'personal_setting.html',
		controller: 'personal_setting'
	}).state('modify_user_name', { //修改用户名
		url: '/modify_user_name',
		templateUrl: 'modify_user_name.html',
		controller: 'modify_user_name'
	}).state('about_us', { //关于我们
		url: '/about_us',
		templateUrl: 'about_us.html',
		controller: 'about_us'
	}).state('help_feedback', { //帮助与反馈
		url: '/help_feedback',
		templateUrl: 'help_feedback.html',
		controller: 'help_feedback'
	}).state('my_invitation', { //我的邀请
		url: '/my_invitation',
		templateUrl: 'my_invitation.html',
		controller: 'my_invitation'
	}).state('voucher_center', { //充值中心
		url: '/voucher_center',
		templateUrl: 'voucher_center.html',
		controller: 'voucher_center'
	}).state('withdrawal_center', { //提现中心
		url: '/withdrawal_center',
		templateUrl: 'withdrawal_center.html',
		controller: 'voucher_center'
	})
	
	
	
	
}])