var firstApp = angular.module("firstapp", ['router', 'dir_iix'])
	.controller("FirstController", function($scope) {

	})
	.controller("shouye", function($scope, sce) { //首页
		$(".sd_uyt_fotter a").removeClass("act").eq(0).addClass("act")
		$scope.banner = '<div class="swiper-slide"><img src="img/index_banner.jpg"></div><div class="swiper-slide"><img src="img/index_banner.jpg"></div>'

	}).controller("discover", function($scope, sce) { //发现

		$scope.banner = '<div class="swiper-slide"><img src="img/banner_a.jpg"></div><div class="swiper-slide"><img src="img/banner_a.jpg"></div>'

		$(".sd_uyt_fotter a").removeClass("act").eq(1).addClass("act")

	}).controller("sound_adapter", function($scope, sce) { //申卡
		$(".sd_uyt_fotter a").removeClass("act").eq(2).addClass("act")
		$scope.banner = '<div class="swiper-slide"><img src="img/ds_banner.png"></div><div class="swiper-slide"><img src="img/ds_banner.png"></div>'

	}).controller("register", function($scope, sce) { //登录
		var typ = 0; // 0登录密码  1验证码登录
		$(".sd_iuu_xsx a").on("click", function() {
			var idx = $(this).index();
			typ = idx
			$(".sd_iuu_xsx a").removeClass("act")
			$(this).addClass("act")
			$(".jjg_iuu_xsrx").removeClass("show").eq(idx).addClass("show")
		})

		$("#dl_iiu_xd").on("click", function() { //登录按钮触发事件
			if(typ == 0) {
				alert("密码登录")
			} else if(typ == 1) {
				alert("验证码登录")
			}
		})

		var d_i = {}
		d_i.type = "1"
		/*		sce.get("http://duxinggj.com:8081/index.php/home/Index/caxun", d_i).then(
					function success(data) {
						console.log(data)
					});*/

	})
	.controller("login", ["$scope", function($scope) { //注册
		$(".sd_xsd_xsrx").on("click", function() {
			$(".chax_oiixd").removeClass("act")
			$(this).find(".chax_oiixd").addClass("act")
		})
	}])
	.controller("forget_password", function($scope, $location) { //忘记密码
		$scope.sjkg_s = ""
		$scope.wj_i = function() { //确认按钮触发
			$scope.sjkg_s = "show"
		}
		/*弹出层方法*/
		$scope.pup = {
			"dl": function() {
				$location.path('register');
			},
			"gb": function() {
				$scope.sjkg_s = ""
			}
		}
	})
	.controller("information", function($scope, sce) { //消息
		$scope.kh_s_a = ["act", "show"];
		$scope.kh_s_b = ["", ""];
		/*客服消息   论坛消息切换*/
		$scope.kg_s = {
			kf: function() {
				$scope.kh_s_a = ["act", "show"];
				$scope.kh_s_b = ["", ""];
			},
			lt: function() {
				$scope.kh_s_a = ["", ""];
				$scope.kh_s_b = ["act", "show"];
			}
		}
	})
	.controller("online_loans", function($scope, sce) { //在线贷款
		$scope.banner = '<div class="swiper-slide"><img src="img/ds_banner.png"></div><div class="swiper-slide"><img src="img/ds_banner.png"></div>'

		$scope.askh_a = ""
		$scope.se_s = function() {
			$scope.askh_a = "act"
		}
		$scope.se_b = function(e) {
			e.stopPropagation()
			$scope.askh_a = ""
		}

	})
	.controller("remind", function($scope, sce) { //提醒

	})
	.controller("course_learning", function($scope, sce) { //课程学习
		$scope.kh_s_a = ["", ""];
		/*客服消息   论坛消息切换*/
		$scope.kg_s = {
			kf: function() {
				$scope.kh_s_a[0] = "act"
				$scope.kh_s_a[1] = ""
			},
			lt: function() {
				$scope.kh_s_a[1] = "act"
				$scope.kh_s_a[0] = ""
			},
			qc: function(e) {
				e.stopPropagation()
				$scope.kh_s_a = ["", ""];
			}
		}
	}).controller("teaching_details", function($scope, sce) { //课程详情

	}).controller("BBS_message", function($scope, sce) { //论坛消息

	}).controller("mine", function($scope, sce) { //我的
		$(".sd_uyt_fotter a").removeClass("act").eq(3).addClass("act")

	}).controller("my_favorite", function($scope, sce) { //我的收藏
		$(".sd_uyt_fotter a").removeClass("act").eq(3).addClass("act");
		$scope.kh_s_a = ["act", "show"];
		$scope.kh_s_b = ["", ""];
		/*课程收藏   帖子切换*/
		$scope.kg_s = {
			kf: function() {
				$scope.kh_s_a = ["act", "show"];
				$scope.kh_s_b = ["", ""];
			},
			lt: function() {
				$scope.kh_s_a = ["", ""];
				$scope.kh_s_b = ["act", "show"];
			}
		}

	})
	.controller("personal_setting", function($scope, sce) { //个人设置
		$(".sd_uyt_fotter a").removeClass("act").eq(3).addClass("act");
		var picker = new mui.PopPicker({ "layer": [3] });
		picker.setData(cityData3)
		$(".szd_ssdf").off("tap").on("tap", function() {
			var hjhg = $(this)
			picker.show(function(rl) {
				$(hjhg).find("span").text(rl[0].text + " " + rl[1].text + " " + rl[2].text)
			})
		})

		var dtPicker = new mui.DtPicker({
			"type": "date",
			"beginYear": "1950" //开始时间

		});
		$('.time').off("tap").on("tap", function() {
			var hgp = $(this)
			dtPicker.show(function(rs) {
				var ye_s = rs.text.split("-")[0],
					da_e = rs.text.split("-")[1].split("-")[0],
					mu_e = rs.text.split("-")[2].split("-")[0]
				$(hgp).find("span").text(ye_s + "年" + da_e + "月" + mu_e + "日")
			})

		})

	}).controller("modify_user_name", function($scope, sce) { //修改用户名

		$(".sd_uyt_fotter a").removeClass("act").eq(3).addClass("act");

	}).controller("about_us", function($scope, sce) { //关于我们

		$(".sd_uyt_fotter a").removeClass("act").eq(3).addClass("act");

	}).controller("help_feedback", function($scope, sce) { //帮助与反馈

		$(".sd_uyt_fotter a").removeClass("act").eq(3).addClass("act");
	}).controller("my_invitation", function($scope, sce) { //我的邀请

		$(".sd_uyt_fotter a").removeClass("act").eq(3).addClass("act");
	}).controller("voucher_center", function($scope, sce) { //充值提现

		$(".sd_uyt_fotter a").removeClass("act").eq(3).addClass("act");
		$scope.kjh_s = true
		$scope.sd_iu=""
		$scope.qs_s=function(){
				$scope.sd_iu=""
		}
		$scope.tijiao_xd = function() { //提交按钮触发方法
			if($scope.kjh_s) {
				var btnArray = ['是', '否'];
				mui.confirm('此提现方式需要绑定微信、是否绑定！', '标题', btnArray, function(e) {
					if(e.index == 0) {
							$scope.kjh_s = false
					} else {

					}
				})
			}else{
				$scope.sd_iu="show"
			}
			
			
		}

	})