var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Macss;
(function (Macss) {
    var Collapse;
    (function (Collapse) {
        var CollapseContainer = /** @class */ (function () {
            function CollapseContainer(openerElementId, showTextElementId, hiddenTextElementId, isDefaultShow) {
                if (isDefaultShow === void 0) { isDefaultShow = true; }
                this.openerId = openerElementId;
                this.showTextId = showTextElementId;
                this.hiddenTextId = hiddenTextElementId;
                $(this.showTextId).hide();
                $(this.hiddenTextId).show();
                if (!isDefaultShow) {
                    $(this.showTextId).show();
                    $(this.hiddenTextId).hide();
                }
            }
            CollapseContainer.prototype.setCollapseEvent = function () {
                var showId = this.showTextId;
                var hiddenId = this.hiddenTextId;
                $(this.openerId).on('shown.bs.collapse', function () {
                    $(showId).hide();
                    $(hiddenId).show();
                });
                $(this.openerId).on('hidden.bs.collapse', function () {
                    $(showId).show();
                    $(hiddenId).hide();
                });
            };
            return CollapseContainer;
        }());
        Collapse.CollapseContainer = CollapseContainer;
        var MenuBar = /** @class */ (function (_super) {
            __extends(MenuBar, _super);
            function MenuBar(openerElementId, showTextElementId, hiddenTextElementId, isDefaultShow) {
                if (isDefaultShow === void 0) { isDefaultShow = true; }
                var _this = _super.call(this, openerElementId, showTextElementId, hiddenTextElementId, isDefaultShow) || this;
                $(_this.showTextId).css("color", "#ddd");
                $(_this.hiddenTextId).css("color", "#ddd");
                return _this;
            }
            return MenuBar;
        }(CollapseContainer));
        Collapse.MenuBar = MenuBar;
        var ConditionsPanel = /** @class */ (function (_super) {
            __extends(ConditionsPanel, _super);
            function ConditionsPanel(openerElementId, showTextElementId, hiddenTextElementId, isDefaultShow) {
                if (isDefaultShow === void 0) { isDefaultShow = true; }
                var _this = _super.call(this, openerElementId, showTextElementId, hiddenTextElementId, isDefaultShow) || this;
                $(_this.showTextId).css("color", "#78bcff");
                $(_this.hiddenTextId).css("color", "#78bcff");
                return _this;
            }
            return ConditionsPanel;
        }(CollapseContainer));
        Collapse.ConditionsPanel = ConditionsPanel;
    })(Collapse = Macss.Collapse || (Macss.Collapse = {}));
})(Macss || (Macss = {}));
//# sourceMappingURL=macss.collapse.js.map