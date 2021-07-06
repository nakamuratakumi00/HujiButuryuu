module Macss.Collapse {
    export class CollapseContainer {

        protected openerId: string;
        protected showTextId: string;
        protected hiddenTextId: string;

        constructor(openerElementId: string, showTextElementId: string, hiddenTextElementId: string, isDefaultShow: boolean = true) {
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

        setCollapseEvent(): void {

            var showId = this.showTextId;
            var hiddenId = this.hiddenTextId;

            $(this.openerId).on('shown.bs.collapse', function () {
                $(showId).hide();
                $(hiddenId).show();
            })
            $(this.openerId).on('hidden.bs.collapse', function () {
                $(showId).show();
                $(hiddenId).hide();
            })
        }
    }

    export class MenuBar extends CollapseContainer {

        constructor(openerElementId: string, showTextElementId: string, hiddenTextElementId: string, isDefaultShow: boolean = true) {
            super(openerElementId, showTextElementId, hiddenTextElementId, isDefaultShow);
            $(this.showTextId).css("color", "#ddd");
            $(this.hiddenTextId).css("color", "#ddd");

        }
    }

    export class ConditionsPanel extends CollapseContainer {

        constructor(openerElementId: string, showTextElementId: string, hiddenTextElementId: string, isDefaultShow: boolean = true) {
            super(openerElementId, showTextElementId, hiddenTextElementId, isDefaultShow);
            $(this.showTextId).css("color", "#78bcff");
            $(this.hiddenTextId).css("color", "#78bcff");

        }
    }
}