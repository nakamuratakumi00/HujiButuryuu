/// <reference path="typings/jquery/jquery.d.ts" />



module Macss.DataTables {

    export class MacssDataTable {

        private id: string;
        private dataTable: any;
        private tableOptions: any;

        constructor(tableElementId: string, paramColumnDefs: any = [{
            "className": "macss-table-proc-column", "width": 90, "orderable": false, "targets": 0
        }], leftColumnNum:number = 1) {
            this.id = tableElementId;

            if (paramColumnDefs == null) {
                paramColumnDefs = [{
                    "className": "macss-table-proc-column", "width": 90, "orderable": false, "targets": 0
                }]
            }

            this.tableOptions = {
                lengthMenu: [5, 10, 20, 50, 100],
                order: [],
                fixedHeader: true,
                pagingType: "simple_numbers",
                scrollX: true,
                scrollY: "360px",
                scrollCollapse: true,
                dom: "<'row macss-table-info-row'<'col-2 macss-table-display-count'l><'col-4'><'col-6 macss-paginate-button'p>>" +
                    "<tr>" +
                    "<'macss-table-info-row'<i><'macss-paginate-button'p>>",
                columnDefs: paramColumnDefs,
                //buttons: [
                //    {
                //        extend: 'colvis',
                //        text: 'カラム切替'
                //    },
                //    {
                //        extend: 'copy',
                //        text: 'Copy to clipboard'
                //    },
                //    'pdf',
                //    'excel',
                //],
                language: {
                    lengthMenu: "表示行数 _MENU_ 件",
                    paginate: {
                        next: "次へ",
                        previous: "前へ"
                    },
                    info: "全_TOTAL_件中 _START_件から_END_件を表示",
                    infoFiltered: ""
                },
                fixedColumns: {
                    leftColumns: leftColumnNum,
                }
            };
        }

        getDatatable(): any {
            return this.dataTable;
        }

        configureDefault(): void {

            var tbl = $(this.id).DataTable();
            this.dataTable = tbl;
        }

        setOption(key: string, value: any): void {
            this.tableOptions[key] = value;
        }

        setLengthMenu(paramLengthMenu: any): void {
            this.setOption("lengthMenu", paramLengthMenu);
        }

        setScrollY(paramScrollY: string): void {
            this.setOption("scrollY", paramScrollY);
        }

        configure(): void {

            var tbl = $(this.id).DataTable(this.tableOptions);
            this.dataTable = tbl;
        }

        configureWithOptions(options: any = {}) {
            var tbl = $(this.id).DataTable(options);
            this.dataTable = tbl;
        }
    }

    export class MacssSearchDialogTable {

        private id: string;
        private dataTable: any;
        private doneConfig: boolean;

        private tableOptions: any;

        constructor(tableElementId: string, paramColumnDefs: any = []) {
            this.id = tableElementId;
            this.doneConfig = false;

            this.tableOptions = {
                lengthMenu: [10, 20, 50, 100],
                searching: false,
                order: [],
                fixedHeader: true,
                pagingType: "simple_numbers",
                scrollX: true,
                scrollY: "300px",
                scrollCollapse: true,
                dom: "<'row macss-table-info-row'<'col-2 macss-table-display-count'l><'col-4'><'col-6 macss-paginate-button'p>>" +
                    "<tr>" +
                    "<'macss-table-info-row'<i><'macss-paginate-button'p>>",
                columnDefs: paramColumnDefs,
                language: {
                    lengthMenu: "表示行数 _MENU_ 件",
                    paginate: {
                        next: "次へ",
                        previous: "前へ"
                    },
                    info: "全_TOTAL_件中 _START_件から_END_件を表示"
                }
            };
        }

        configure(paramScrollY: string = "300px"): void {

            if (this.doneConfig) {
                return;
            }

            this.tableOptions["scrollY"] = paramScrollY;

            this.dataTable = $(this.id).DataTable(this.tableOptions);
            this.doneConfig = true;
        }

        init(): void {
            if (this.doneConfig) {
                this.dataTable.state.clear();
                this.dataTable.destroy();
            }

            $(this.id + " tbody").empty();
        }

        refresh(): void {
            this.dataTable = $(this.id).DataTable(this.tableOptions);
            this.doneConfig = true;
        }
    }

    ///
    /// リストのみ（ページャーなどがありません）
    ///
    export class MacssSimpleTable {

        private id: string;
        private dataTable: any;
        private tableOptions: any;

        constructor(tableElementId: string, order: boolean = false) {
            this.id = tableElementId;

            this.tableOptions = {
                ordering: order,
                fixedHeader: false,
                paging: false,
                scrollX: true,
                scrollY: "360px",
                scrollCollapse: true,
                dom: "<'col-4'>"
            };
        }

        getDatatable(): any {
            return this.dataTable;
        }

        configureDefault(): void {

            var tbl = $(this.id).DataTable();
            this.dataTable = tbl;
        }

        setOption(key: string, value: any): void {
            this.tableOptions[key] = value;
        }

        setScrollY(paramScrollY: string): void {
            this.setOption("scrollY", paramScrollY);
        }

        configure(): void {

            var tbl = $(this.id).DataTable(this.tableOptions);
            this.dataTable = tbl;
        }

        configureWithOptions(options: any = {}) {
            var tbl = $(this.id).DataTable(options);
            this.dataTable = tbl;
        }
    }

}