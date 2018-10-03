Ext.ux.TreeCombo = Ext.extend(Ext.form.ComboBox, {
    constructor: function (cfg) {
        cfg = cfg || {};
        Ext.ux.TreeCombo.superclass.constructor.call(this, Ext.apply({
            maxHeight: 300,
            editable: false,
            mode: 'local',
            triggerAction: 'all',
            rootVisible: false,
            selectMode: 'all'
        }, cfg));
    },
    store: new Ext.data.SimpleStore({
        fields: [],
        data: [[]]
    }),
    // 重写onViewClick，使展开树结点是不关闭下拉框 
    onViewClick: function (doFocus) {
        var index = this.view.getSelectedIndexes()[0], s = this.store, r = s.getAt(index);
        if (r) {
            this.onSelect(r, index);
        }
        if (doFocus !== false) {
            this.el.focus();
        }
    },
    tree: null,
    // 隐藏值 
    hiddenValue: null,
    getHiddenValue: function () {
        return this.hiddenValue;
    },
    getValue: function () { //增加适用性，与原来combo组件一样 
        return this.hiddenValue;
    },
    setHiddenValue: function (code, dispText) {
        this.setValue(code);
        Ext.form.ComboBox.superclass.setValue.call(this, dispText);
        this.hiddenValue = code;
    },
    initComponent: function () {
        var _this = this;
        var tplRandomId = 'deptcombo_' + Math.floor(Math.random() * 1000) + this.tplId
        this.tpl = "<div style='height:" + _this.maxHeight + "px' id='" + tplRandomId + "'></div>"
        this.tree = new Ext.tree.TreePanel({
            border: false,
            enableDD: false,
            enableDrag: false,
            rootVisible: _this.rootVisible || false,
            autoScroll: true,
            trackMouseOver: true,
            height: _this.maxHeight,
            lines: true,
            singleExpand: true,
            root: new Ext.tree.AsyncTreeNode({
                id: _this.rootId,
                text: _this.rootText,
                iconCls: 'ico-root',
                expanded: true,
                leaf: false,
                border: false,
                draggable: false,
                singleClickExpand: false,
                hide: true
            }),
            loader: new Ext.tree.TreeLoader({
                nodeParameter: 'ID',
                requestMethod: 'GET',
                dataUrl: _this.url
            })
        });
        this.tree.on('click', function (node) {
            if ((_this.selectMode == 'leaf' && node.leaf == true) || _this.selectMode == 'all') {
                if (_this.fireEvent('beforeselect', _this, node)) {
                    _this.fireEvent('select', _this, node);
                }
            }
        });
        this.on('select', function (obj, node) {
            var dispText = node.text;
            var code = node.id;
            obj.setHiddenValue(code, dispText);
            obj.collapse();
        });
        this.on('expand', function () {
            this.tree.render(tplRandomId);
        });
        Ext.ux.TreeCombo.superclass.initComponent.call(this);
    }
})
Ext.reg("treecombo", Ext.ux.TreeCombo);