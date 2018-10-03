Ext.onReady(function () {
//    var _window = new Ext.Window({
//        title: "查询条件",
//        renderTo:Ext.getBody(),
//        frame: true,
//        plain: false,
//        buttonAlign: "center",
//        closeAction: "hide",
//        maximizable: false,
//        closable: true,
//        bodyStyle: "padding:20px",
//        width: 350,
//        height: 300,
//        layout: "form",
//        lableWidth: 110,
//        defaults: { xtype: "textfield", width: 180 },
//        items: [
//                {
//                    xtype: "treecombo",
//                    fieldLabel: "商户类别",
//                    anchor: "100%",
//                    url: "http://localhost:2779/ShopCategory/Product/listseason"/// <reference path="../treeData.ashx" /> 
//                }
//             ],
//        buttons: [{ text: "保 存" }, { text: "取消", handler: function () { _window.hide(); } }]
//    });
    //     _window.show();
    var _window = new Ext.Window({
        title: "查询条件",
        renderTo: Ext.getBody(),
        frame: true,
        plain: true,
        buttonAlign: "center",
        closeAction: "hide",
        maximizable: true,
        closable: true,
        bodyStyle: "padding:20px",
        width: 350,
        height: 300,
        layout: "form",
        lableWidth: 110,
        defaults: { xtype: "textfield", width: 180 },
        items: [
{
    fieldLabel: "案件编号",
    anchor: "100%"
},
{
    xtype: "datefield",
    fieldLabel: "案发时间",
    anchor: "100%"
},
{
    fieldLabel: "举报人",
    anchor: "100%"
},
{
    fieldLabel: "被举报单位或个人",
    anchor: "100%"
},
{
    xtype: "treecombo",
    fieldLabel: "案件发生地",
    anchor: "100%",
    url: "http://localhost:2779/ShopCategory/Product/listseason"/// <reference path="../treeData.ashx" /> 
},
{
    xtype: "treecombo",
    fieldLabel: "案件类型",
    anchor: "100%",
    url: "http://localhost:1502/window/ajwflx.ashx"
},
{
    xtype: "treecombo",
    fieldLabel: "案件性质",
    anchor: "100%",
    url: "http://localhost:1502/window/ajwfxz.ashx"
}
],
        buttons: [{ text: "确定" }, { text: "取消", handler: function () { _window.hide(); } }]
    })
    _window.show(); 

});
    

