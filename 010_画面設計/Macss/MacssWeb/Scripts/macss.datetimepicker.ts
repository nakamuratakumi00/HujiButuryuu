module Macss.DatetimePicker {

    export class MacssDtPicker {
        private id: string;
        private triggerId: string;

        constructor(dtPickerElementId: string, triggerElementId: string) {
            this.id = dtPickerElementId;
            this.triggerId = triggerElementId;
        }

        configure(): void {

            $(this.id).datetimepicker({
                lang: "ja",
                timepicker: false,
                format: "Y/m/d",
                scrollMonth: false,
                scrollInput: false  //2020.12.21追加 スクロール時の月変更不可、スクロール時の自動入力不可に変更
            });

            var targetId = this.id;
            $(this.triggerId).click(function () {
                $(targetId).datetimepicker('show');
            })
        }
    }
}
