function isCode(val) {
    var myreg = /^\d{6}$/;
    return (myreg.test(val));
}

function formatDateNew(v) {
    if (typeof v == "string") {
        v = parseDate(v);
    }
    if (v instanceof Date) {
        var y = v.getFullYear();
        var m = v.getMonth() + 1;
        m = m < 10 ? "0" + m : m;
        var d = v.getDate();
        d = d < 10 ? "0" + d : d;
        var h = v.getHours();
        h = h < 10 ? "0" + h : h;
        var i = v.getMinutes();
        i = i < 10 ? "0" + i : i;
        return y + "-" + m + "-" + d + " " + h + ":" + i;
    }
    return "";
}

function dateAddDay(_date, day) {
    if (day == "")
        day = 0;
    var time = _date.split("-");

    var now = new Date(time[0], time[1] - 1, time[2]);
    var milliseconds = day * 1000 * 60 * 60 * 24;
    var testdate = milliseconds + now.getTime();
    var testDate = new Date();
    testDate.setTime(testdate);
    var formateDate = formatDateNew(testDate);
    var formateDateStr = formateDate.substring(0, 10);
    return formateDateStr;
}
function dateAddMinute(_date, minute) {

    if (minute == "")
        minute = 0;
    var timeArray = _date.split(" ");
    var time = timeArray[0].split("-");
    var hour = timeArray[1].split(":");
    var now = new Date(time[0], time[1] - 1, time[2], hour[0], hour[1]);
    var milliseconds = minute * 1000 * 60;
    var testdate = milliseconds + now.getTime();
    var testDate = new Date();
    testDate.setTime(testdate);

    return formatDateNew(testDate);
}
function convertDate(_date, day) {
    var time = _date.split("-");

    var now = new Date(time[0], time[1] - 1, time[2]);
    var milliseconds = day * 1000 * 60 * 60 * 24;
    var testdate = milliseconds + now.getTime();
    var testDate = new Date();
    testDate.setTime(testdate);
    return testDate;
}
function dateAddMonths(_date, value) {
    var pDate, aDate, tDate, oDate1;
    pDate = _date.split(" ");
    aDate = pDate[0].split("-");
    tDate = pDate[1].split(":");
    oDate1 = new Date(aDate[0], aDate[1] - 1, aDate[2], tDate[0], tDate[1]);
    var date = oDate1.setMonth(oDate1.getMonth() + value);
    var limitdate = new Date(date);
    return formatDateNew(limitdate);
}

function DateDiffToDay(sDate, eDate) {
    var pDate, aDate, tDate, oDate1, oDate2, iDays;
    pDate = sDate.split(" ");
    aDate = pDate[0].split("-");
    tDate = pDate[1].split(":");
    oDate1 = new Date(aDate[0], aDate[1] - 1, aDate[2], tDate[0], tDate[1]);
    pDate = eDate.split(" ");
    aDate = pDate[0].split("-");
    tDate = pDate[1].split(":");
    oDate2 = new Date(aDate[0], aDate[1] - 1, aDate[2], tDate[0], tDate[1]);
    iDays = (oDate1 - oDate2) / 1000 / 60 / 60 / 24.0;
    iDays = Math.ceil(iDays);
    return iDays;
}

function DateDiffToMinute(sDate, eDate) {
    var pDate, aDate, tDate, oDate1, oDate2, minutes;
    pDate = sDate.split(" ");
    aDate = pDate[0].split("-");
    tDate = pDate[1].split(":");
    oDate1 = new Date(aDate[0], aDate[1] - 1, aDate[2], tDate[0], tDate[1]);
    pDate = eDate.split(" ");
    aDate = pDate[0].split("-");
    tDate = pDate[1].split(":");
    oDate2 = new Date(aDate[0], aDate[1] - 1, aDate[2], tDate[0], tDate[1]);
    minutes = (oDate1 - oDate2) / 1000 / 60;
    return minutes;
}

function DateDiffToHour(sDate, eDate) {
    var pDate, aDate, tDate, oDate1, oDate2, hours;
    pDate = sDate.split(" ");
    aDate = pDate[0].split("-");
    tDate = pDate[1].split(":");
    oDate1 = new Date(aDate[0], aDate[1] - 1, aDate[2], tDate[0], tDate[1]);
    pDate = eDate.split(" ");
    aDate = pDate[0].split("-");
    tDate = pDate[1].split(":");
    oDate2 = new Date(aDate[0], aDate[1] - 1, aDate[2], tDate[0], tDate[1]);
    hours = (oDate1 - oDate2) / 1000 / 60 / 60;
    return hours;
}
//验证EMAIL 
function isEmail(val) {
    var pass = 0;
    if (window.RegExp) {
        var tempS = "a";
        var tempReg = new RegExp(tempS);
        if (tempReg.test(tempS)) {

            pass = 1;
        }
    }
    if (!pass) {

        return (val.indexOf(".") > 2) && (val.indexOf("@") > 0);
    }
    var r1 = new RegExp("(@.*@)|(\\.\\.)|(@\\.)|(^\\.)");
    var r2 = new RegExp("^[a-zA-Z0-9\\.\\!\\#\\$\\%\\&\\??\\*\\+\\-\\/\\=\\?\\^\\_\\`\\{\\}\\~]*[a-zA-Z0-9\\!\\#\\$\\%\\&\\??\\*\\+\\-\\/\\=\\?\\^\\_\\`\\{\\}\\~]\\@(\\[?)[a-zA-Z0-9\\-\\.]+\\.([a-zA-Z]{2,3})(\\]?)$");
    return (!r1.test(val) && r2.test(val));
}
//去掉字符串头尾空格 luzhonghua
function trim(str) {
    var strReturn;
    strReturn = leftTrim(str);
    strReturn = rightTrim(strReturn);
    return strReturn;
}

//去掉字符串头空格  luzhonghua
function leftTrim(strValue) {
    if (!strValue) {
        return strValue;
    }
    var re = /^\s*/;

    return strValue.replace(re, "");
}
//邮政编码验证
function isPostcode(val) {
    var myreg = /^[0-9]\d{5}$/;
    return (myreg.test(val));
}

//去掉字符串尾空格  luzhonghua
function rightTrim(strValue) {
    var re = /\s*$/;
    if (strValue == null)
        return null;

    return strValue.replace(re, "");
}
//验证是否有非法字符
function checkStr(str) {
    var flag = false;
    var checkString = "`~!@#$%^&*()+=[]{}\\|;':\",./<>?";
    //if(typeof(str)!="undefined")checkString=str;
    for (var j = 0; j < checkString.length; j++) {
        if (str.indexOf(checkString.substring(j, j + 1)) != -1) {
            alert("不能有非法字符:\"" + checkString.substring(j, j + 1) + "\"");
            flag = true;
            break;
        }
    }
    return flag;
}


//验证银行卡号
function isIdentity(val) {
    var myreg = /^([1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3})|([1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}([0-9]|X))$/;
    return (myreg.test(val));
}

//验证银行卡号
function isCardNumber(val) {
    var myreg = /^(\d{12})|(\d{15})|(\d{19})$/;
    return (myreg.test(val));
}
//验证手机号
function isMobile(val) {
    var myreg = /^(((13[0-9]{1})|(15[0-9]{1})|(14[0-9]{1})|(17[0-9]{1})|(18[0-9]{1}))+\d{8})$/;
    return (myreg.test(val));
}
//对输入域是否是整数的校验,即只包含字符0123456789 
function isInteger(strValue) {
    return regExpTest(strValue, /\d+/);
}

//RegExt Test  
function regExpTest(source, re) {
    var result = false;

    if (source == null || source == "")
        return false;

    if (source == re.exec(source))
        result = true;

    return result;
}

/**
* 实现MAP功能
* @param {Object} key
* @param {Object} value
* @memberOf {TypeName} 
*/
function struct(key, value) {
    this.key = key;
    this.value = value;
}

function put(key, value) {
    for (var i = 0; i < this.arr.length; i++) {
        if (this.arr[i].key === key) {
            this.arr[i].value = value;
            return;
        }
    }
    this.arr[this.arr.length] = new struct(key, value);
}

function get(key) {
    for (var i = 0; i < this.arr.length; i++) {
        if (this.arr[i].key === key) {
            return this.arr[i].value;
        }
    }
    return null;
}

function remove(key) {
    var v;
    for (var i = 0; i < this.arr.length; i++) {
        v = this.arr.pop();
        if (v.key === key) {
            continue;
        }
        this.arr.unshift(v);
    }
}

function size() {
    return this.arr.length;
}

function isEmpty() {
    return this.arr.length <= 0;
}

function checkEmpty(obj) {
    if (typeof (obj) != "number")
        if (obj == null || obj == "" || obj == undefined || obj == 'null') {
            return "";
        } else {
            return obj;
        }
    else
        return obj;
}

//function sort(){
//	
//}

function Map() {
    this.arr = new Array();
    this.get = get;
    this.put = put;
    this.remove = remove;
    this.size = size;
    this.isEmpty = isEmpty;
    //   this.sort = sort;   
}
String.prototype.Tlength = function () {
    var arr = this.match(/[^\x00-\xff]/ig);
    return this.length + (arr == null ? 0 : arr.length);
};

Array.prototype.remove = function (dx) {
    if (isNaN(dx) || dx > this.length) {
        return false;
    }
    for (var i = 0, n = 0; i < this.length; i++) {
        if (this[i] != this[dx]) {
            this[n++] = this[i];
        }
    }
    this.length -= 1;
};
Array.prototype.getIndexByValue = function (value) {
    var index = -1;
    for (var i = 0; i < this.length; i++) {
        if (this[i] == value) {
            index = i;
            break;
        }
    }
    return index;
};

//判断浏览器
window["MzBrowser"] = {};
(function () {
    if (MzBrowser.platform)
        return;
    var ua = window.navigator.userAgent;
    MzBrowser.platform = window.navigator.platform;
    MzBrowser.firefox = ua.indexOf("Firefox") > 0;
    MzBrowser.opera = typeof (window.opera) == "object";
    MzBrowser.ie = !MzBrowser.opera && ua.indexOf("MSIE") > 0;
    MzBrowser.mozilla = window.navigator.product == "Gecko";
    MzBrowser.netscape = window.navigator.vendor == "Netscape";
    MzBrowser.safari = ua.indexOf("Safari") > -1;
    if (MzBrowser.firefox)
        var re = /Firefox(\s|\/)(\d+(\.\d+)?)/;
    else if (MzBrowser.ie)
        var re = /MSIE( )(\d+(\.\d+)?)/;
    else if (MzBrowser.opera)
        var re = /Opera(\s|\/)(\d+(\.\d+)?)/;
    else if (MzBrowser.netscape)
        var re = /Netscape(\s|\/)(\d+(\.\d+)?)/;
    else if (MzBrowser.safari)
        var re = /Version(\/)(\d+(\.\d+)?)/;
    else if (MzBrowser.mozilla)
        var re = /rv(\:)(\d+(\.\d+)?)/;
    if ("undefined" != typeof (re) && re.test(ua))
        MzBrowser.version = parseFloat(RegExp.$2);
})();



function getDateImg(selectDate) {
    var image;
    var currentDate = new Date();
    var day = DateDiffToDay(selectDate + " 00:00", formatDateNew(currentDate));
    if (day >= 0 && day <= 2) {
        image = dateImageMap.get(day);
        if (image != null) {
            image = carwrmimageURL + image;
        }
    } else {
        var selectDateD = convertDate(selectDate, 0);
        if (selectDateD instanceof Date) {
            var weeek = selectDateD.getDay();
            image = weekImageMap.get(weeek);
            if (image != null) {
                image = carwrmimageURL + image;
            }
        }

    }
    return image;
}
/**
* 验证身份证是否过期
* @returns {Boolean}
*/
function checkIdentitycloseday() {
    var overIdentitycloseday = false;
    var identitycloseday = $('#identitycloseday').val();
    if (identitycloseday) {
        identitycloseday = identitycloseday + " 00:00";
        var currentTime = $('#currentDateStr').val();
        if (!currentTime) {
            currentTime = formatDateNew(new Date());
        }
        if (DateDiffToMinute(identitycloseday, currentTime) < 0) {
            overIdentitycloseday = true;
        }
    }
    return overIdentitycloseday;
}

//读取用户cookie信息
function getCookieValue(name) {
    var cookies = document.cookie;
    if (cookies.length > 0) {
        var cookieArray = cookies.split(";");
        if (cookieArray.length > 0) {
            for (var i = 0; i < cookieArray.length; i++) {
                var cookie = cookieArray[i];
                if (cookie.indexOf(name) > -1) {
                    var value = cookie.substring(cookie.indexOf("=") + 1, cookie.length);
                    return value;
                }
            }
        }
    }
}


