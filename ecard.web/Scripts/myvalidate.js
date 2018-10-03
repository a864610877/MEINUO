
/** 
* 数据验证框架.增加了对id字段检查出错时,直接在对应后面添加一< span>元素来显示错误信息. 
* 
* @author wangzi6hao 
* @version 2.1 
* @description 2009-05-16 
*/
var checkData = new function () {
    var idExt = "_wangzi6hao_Span"; //生成span层的id后缀 
    /** 
    * 得到中英文字符长(中文为2个字符) 
    * 
    * @param {} 
    * str 
    * @return 字符长 
    */
    this.length = function (str) {
        var p1 = new RegExp('%u..', 'g')
        var p2 = new RegExp('%.', 'g')
        return escape(str).replace(p1, '').replace(p2, '').length
    }
    /** 
    * 删除对应id元素 
    */
    this.remove = function (id) {
        var idObject = document.getElementById(id);
        if (idObject != null)
            idObject.parentNode.removeChild(idObject);
    }
    /** 
    * 在对应id后面错误信息 
    * 
    * @param id:需要显示错误信息的id元素 
    * str:显示错误信息 
    */
    this.appendError = function (id, str) {
        this.remove(id + idExt); // 如果span元素存在，则先删除此元素 
        var spanNew = document.createElement("span"); // 创建span 
        spanNew.id = id + idExt; // 生成spanid 
        spanNew.style.color = "red";
        spanNew.appendChild(document.createTextNode(str)); // 给span添加内容 
        var inputId = document.getElementById(id);
        inputId.parentNode.insertBefore(spanNew, inputId.nextSibling); // 给需要添加元素后面添加span 
    }
    /** 
    * @description 过滤所有空格字符。 
    * @param str:需要去掉空间的原始字符串 
    * @return 返回已经去掉空格的字符串 
    */
    this.trimSpace = function (str) {
        str += "";
        while ((str.charAt(0) == ' ') || (str.charAt(0) == '???')
|| (escape(str.charAt(0)) == '%u3000'))
            str = str.substring(1, str.length);
        while ((str.charAt(str.length - 1) == ' ')
|| (str.charAt(str.length - 1) == '???')
|| (escape(str.charAt(str.length - 1)) == '%u3000'))
            str = str.substring(0, str.length - 1);
        return str;
    }
    /** 
    * 过滤字符串开始部分的空格\字符串结束部分的空格\将文字中间多个相连的空格变为一个空格 
    * 
    * @param {Object} 
    * inputString 
    */
    this.trim = function (inputString) {
        if (typeof inputString != "string") {
            return inputString;
        }
        var retValue = inputString;
        var ch = retValue.substring(0, 1);
        while (ch == " ") {
            // 检查字符串开始部分的空格 
            retValue = retValue.substring(1, retValue.length);
            ch = retValue.substring(0, 1);
        }
        ch = retValue.substring(retValue.length - 1, retValue.length);
        while (ch == " ") {
            // 检查字符串结束部分的空格 
            retValue = retValue.substring(0, retValue.length - 1);
            ch = retValue.substring(retValue.length - 1, retValue.length);
        }
        while (retValue.indexOf(" ") != -1) {
            // 将文字中间多个相连的空格变为一个空格 
            retValue = retValue.substring(0, retValue.indexOf(" "))
+ retValue.substring(retValue.indexOf(" ") + 1,
retValue.length);
        }
        return retValue;
    }
    /** 
    * 过滤字符串,指定过滤内容，如果内容为空，则默认过滤 '~!@#$%^&*()-+." 
    * 
    * @param {Object} 
    * str 
    * @param {Object} 
    * filterStr 
    * 
    * @return 包含过滤内容，返回True,否则返回false; 
    */
    this.filterStr = function (str, filterString) {
        filterString = filterString == "" ? "'~!@#$%^&*()-+.\"" : filterString
        var ch;
        var i;
        var temp;
        var error = false; // 当包含非法字符时，返回True 
        for (i = 0; i <= (filterString.length - 1); i++) {
            ch = filterString.charAt(i);
            temp = str.indexOf(ch);
            if (temp != -1) {
                error = true;
                break;
            }
        }
        return error;
    }
    this.filterStrSpan = function (id, filterString) {
        filterString = filterString == "" ? "'~!@#$%^&*()-+.\"" : filterString
        var val = document.getElementById(id);
        if (this.filterStr(val.value, filterString)) {
            val.select();
            var str = "不能包含非法字符" + filterString;
            this.appendError(id, str);
            return false;
        } else {
            this.remove(id + idExt);
            return true;
        }
    }
    /** 
    * 检查是否为网址 
    * 
    * @param {} 
    * str_url 
    * @return {Boolean} true：是网址，false:<b>不是</b>网址; 
    */
    this.isURL = function (str_url) {// 验证url 
        var strRegex = "^((https|http|ftp|rtsp|mms)?://)"
+ "?(([0-9a-z_!~*'().&=+$%-]+: )?[0-9a-z_!~*'().&=+$%-]+@)?" // ftp的user@ 
+ "(([0-9]{1,3}\.){3}[0-9]{1,3}" // IP形式的URL- 199.194.52.184 
+ "|" // 允许IP和DOMAIN（域名） 
+ "([0-9a-z_!~*'()-]+\.)*" // 域名- www. 
+ "([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]\." // 二级域名 
+ "[a-z]{2,6})" // first level domain- .com or .museum 
+ "(:[0-9]{1,4})?" // 端口- :80 
+ "((/?)|" // a slash isn't required if there is no file name 
+ "(/[0-9a-z_!~*'().;?:@&=+$,%#-]+)+/?)$";
        var re = new RegExp(strRegex);
        return re.test(str_url);
    }
    this.isURLSpan = function (id) {
        var val = document.getElementById(id);
        if (!this.isURL(val.value)) {
            val.select();
            var str = "链接不符合格式;";
            this.appendError(id, str);
            return false;
        } else {
            this.remove(id + idExt);
            return true;
        }
    }
    /** 
    * 检查是否为电子邮件 
    * 
    * @param {} 
    * str 
    * @return {Boolean} true：电子邮件，false:<b>不是</b>电子邮件; 
    */
    this.isEmail = function (str) {
        var re = /^([a-zA-Z0-9]+[_|\-|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\-|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$/;
        return re.test(str);
    }
    this.isEmailSpan = function (id) {
        var val = document.getElementById(id);
        if (!this.isEmail(val.value)) {
            val.select();
            var str = "邮件不符合格式;";
            this.appendError(id, str);
            return false;
        } else {
            this.remove(id + idExt);
            return true;
        }
    }
    /** 
    * 检查是否为数字 
    * 
    * @param {} 
    * str 
    * @return {Boolean} true：数字，false:<b>不是</b>数字; 
    */
    this.isNum = function (str) {
        var re = /^[\d]+$/
        return re.test(str);
    }
    this.isNumSpan = function (id) {
        var val = document.getElementById(id);
        if (!this.isNum(val.value)) {
            val.select();
            var str = "必须是数字;";
            this.appendError(id, str);
            return false;
        } else {
            this.remove(id + idExt);
            return true;
        }
    }
    /** 
    * 检查数值是否在给定范围以内,为空,不做检查<br> 
    * 
    * @param {} 
    * str_num 
    * @param {} 
    * small 应该大于或者等于的数值（此值为空时，只检查不能大于最大值） 
    * @param {} 
    * big 应该小于或者等于的数值（此值为空时，只检查不能小于最小值） 
    * 
    * @return {Boolean} <b>小于最小数值或者大于最大数值</b>数字返回false 否则返回true; 
    */
    this.isRangeNum = function (str_num, small, big) {
        if (!this.isNum(str_num)) // 检查是否为数字 
            return false
        if (small == "" && big == "")
            throw str_num + "没有定义最大，最小值数字！";
        if (small != "") {
            if (str_num < small)
                return false;
        }
        if (big != "") {
            if (str_num > big)
                return false;
        }
        return true;
    }
    this.isRangeNumSpan = function (id, small, big) {
        var val = document.getElementById(id);
        if (!this.isRangeNum(val.value, small, big)) {
            val.select();
            var str = "";
            if (small != "") {
                str = "应该大于或者等于 " + small;
            }
            if (big != "") {
                str += " 应该小于或者等于 " + big;
            }
            this.appendError(id, str);
            return false;
        } else {
            this.remove(id + idExt);
            return true;
        }
    }
    /** 
    * 检查是否为合格字符串(不区分大小写)<br> 
    * 是由a-z0-9_组成的字符串 
    * 
    * @param {} 
    * str 检查的字符串 
    * @param {} 
    * idStr 光标定位的字段ID<b>只能接收ID</b> 
    * @return {Boolean} <b>不是</b>"a-z0-9_"组成返回false,否则返回true; 
    */
    this.isLicit = function (str) {
        var re = /^[_0-9a-zA-Z]*$/
        return re.test(str);
    }
    this.isLicitSpan = function (id) {
        var val = document.getElementById(id);
        if (!this.isLicit(val.value)) {
            val.select();
            var str = "是由a-z0-9_组成的字符串(不区分大小写);";
            this.appendError(id, str);
            return false;
        } else {
            this.remove(id + idExt);
            return true;
        }
    }
    /** 
    * 检查二个字符串是否相等 
    * 
    * @param {} 
    * str1 第一个字符串 
    * @param {} 
    * str2 第二个字符串 
    * @return {Boolean} 字符串不相等返回false,否则返回true; 
    */
    this.isEquals = function (str1, str2) {
        return str1 == str2;
    }
    this.isEqualsSpan = function (id, id1) {
        var val = document.getElementById(id);
        var val1 = document.getElementById(id1);
        if (!this.isEquals(val.value, val1.value)) {
            val.select();
            var str = "二次输入内容必须一样;";
            this.appendError(id, str);
            return false;
        } else {
            this.remove(id + idExt);
            return true;
        }
    }
    /** 
    * 检查字符串是否在给定长度范围以内(中文字符以2个字节计算),字符为空,不做检查<br> 
    * 
    * @param {} 
    * str 检查的字符 
    * @param {} 
    * lessLen 应该大于或者等于的长度 
    * @param {} 
    * moreLen 应该小于或者等于的长度 
    * 
    * @return {Boolean} <b>小于最小长度或者大于最大长度</b>数字返回false; 
    */
    this.isRange = function (str, lessLen, moreLen) {
        var strLen = this.length(str);
        if (lessLen != "") {
            if (strLen < lessLen)
                return false;
        }
        if (moreLen != "") {
            if (strLen > moreLen)
                return false;
        }
        if (lessLen == "" && moreLen == "")
            throw "没有定义最大最小长度!";
        return true;
    }
    this.isRangeSpan = function (id, lessLen, moreLen) {
        var val = document.getElementById(id);
        if (!this.isRange(val.value, lessLen, moreLen)) {
            var str = "长度";
            if (lessLen != "")
                str += "大于或者等于 " + lessLen + ";";
            if (moreLen != "")
                str += " 应该小于或者等于 " + moreLen;
            val.select();
            this.appendError(id, str);
            return false;
        } else {
            this.remove(id + idExt);
            return true;
        }
    }
    /** 
    * 检查字符串是否小于给定长度范围(中文字符以2个字节计算)<br> 
    * 
    * @param {} 
    * str 字符串 
    * @param {} 
    * lessLen 小于或等于长度 
    * 
    * @return {Boolean} <b>小于给定长度</b>数字返回false; 
    */
    this.isLess = function (str, lessLen) {
        return this.isRange(str, lessLen, "");
    }
    this.isLessSpan = function (id, lessLen) {
        var val = document.getElementById(id);
        if (!this.isLess(val.value, lessLen)) {
            var str = "长度大于或者等于 " + lessLen;
            val.select();
            this.appendError(id, str);
            return false;
        } else {
            this.remove(id + idExt);
            return true;
        }
    }
    /** 
    * 检查字符串是否大于给定长度范围(中文字符以2个字节计算)<br> 
    * 
    * @param {} 
    * str 字符串 
    * @param {} 
    * moreLen 小于或等于长度 
    * 
    * @return {Boolean} <b>大于给定长度</b>数字返回false; 
    */
    this.isMore = function (str, moreLen) {
        return this.isRange(str, "", moreLen);
    }
    this.isMoreSpan = function (id, moreLen) {
        var val = document.getElementById(id);
        if (!this.isMore(val.value, moreLen)) {
            var str = "长度应该小于或者等于 " + moreLen;
            val.select();
            this.appendError(id, str);
            return false;
        } else {
            this.remove(id + idExt);
            return true;
        }
    }
    /** 
    * 检查字符不为空 
    * 
    * @param {} 
    * str 
    * @return {Boolean} <b>字符为空</b>返回true,否则为false; 
    */
    this.isEmpty = function (str) {
        return str == "";
    }
    this.isEmptySpan = function (id) {
        var val = document.getElementById(id);
        if (this.isEmpty(val.value)) {
            var str = "不允许为空;";
            val.select();
            this.appendError(id, str);
            return false;
        } else {
            this.remove(id + idExt);
            return true;
        }
    }
} 