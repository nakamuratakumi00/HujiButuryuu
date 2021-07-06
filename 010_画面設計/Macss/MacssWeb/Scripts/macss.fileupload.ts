module Macss.FileUpload {

    export class MacssFileUpload {

        private uploadElement: string;
        private labelElement: string;
        private multipleUpload: boolean;

        constructor(uploadElementSelector: string = ".custom-file-input"
            , uploadLabelElementSelector: string = ".custom-file-label"
            , multiple = false) {
            this.uploadElement = uploadElementSelector;
            this.labelElement = uploadLabelElementSelector;
            this.multipleUpload = multiple;
        }

        configure(): void {

            var upload = this.uploadElement;
            var label = this.labelElement;

            //$(this.uploadElement).on('change', function () {
            //    var fileElement: any = $(upload)[0];
            //    $(upload).next(label).html(fileElement.files[0].name);
            //})
            $(this.uploadElement).on('change', (e: JQueryEventObject) => {
                //var fileElement: any = this.getFile(); 
                var str = "";
                if (this.multipleUpload) {
                    var num = this.getFileNum();
                    for (var i = 0; i < num; i++) {
                        str += this.getFileName(0, i)
                        str += ", ";
                    }
                }
                else {
                    str = this.getFileName();
                }
                $(this.uploadElement).next(this.labelElement).html(str);
            });

        }

        getFileNum():number {
            var elem = this.getFileElement(0);
            return elem.files.length;
        }

        getFileElement(elementIndex: number = 0): any {
            return $(this.uploadElement)[elementIndex];
        }

        getFile(elementIndex: number = 0, fileIndex: number = 0): any {
            var elem = this.getFileElement(elementIndex);
            return elem.files[fileIndex];
        }

        getFileName(elementIndex: number = 0, fileIndex: number = 0): string {
            var f = this.getFile(elementIndex, fileIndex);
            if (f == null) {
                return "";
            }
            return f.name;
        }

        getFileExt(elementIndex: number = 0, fileIndex: number = 0): string {
            var f = this.getFileName(elementIndex, fileIndex);

            var idx = f.lastIndexOf(".");

            if (idx < 0 || idx >= (f.length - 1)) {
                return "";
            }

            var ext = f.substring(idx);

            return ext;
        }

        getFileSize(elementIndex: number = 0, fileIndex: number = 0): number {
            var f = this.getFile(elementIndex, fileIndex);
            return f.size;
        }

        acceptExt(acceptList: Array<string>, elementIndex: number = 0, fileIndex: number = 0):boolean {

            var ext = this.getFileExt(elementIndex, fileIndex);

            var idx = acceptList.indexOf(ext);

            return idx >= 0;
        }

        withinRangeOfFileSize(fileSize: number, elementIndex: number = 0, fileIndex: number = 0): boolean {

            var fSize = this.getFileSize(elementIndex, fileIndex);

            return fSize < fileSize;
        }
    }
}