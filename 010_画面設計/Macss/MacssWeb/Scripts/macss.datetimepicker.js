var Macss;
(function (Macss) {
    var DatetimePicker;
    (function (DatetimePicker) {
        var MacssDtPicker = /** @class */ (function () {
            function MacssDtPicker(dtPickerElementId, triggerElementId) {
                this.id = dtPickerElementId;
                this.triggerId = triggerElementId;
            }
            MacssDtPicker.prototype.configure = function () {
                $(this.id).datetimepicker({
                    lang: "ja",
                    timepicker: false,
                    format: "Y/m/d",
                    scrollMonth: false,
                    scrollInput: false //2020.12.21追加 スクロール時の月変更不可、スクロール時の自動入力不可に変更
                });
                var targetId = this.id;
                $(this.triggerId).click(function () {
                    $(targetId).datetimepicker('show');
                });
            };
            return MacssDtPicker;
        }());
        DatetimePicker.MacssDtPicker = MacssDtPicker;
    })(DatetimePicker = Macss.DatetimePicker || (Macss.DatetimePicker = {}));
})(Macss || (Macss = {}));
//# sourceMappingURL=macss.datetimepicker.js.map