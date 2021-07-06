(function ($) {
    $.extend($.fn.bootstrapTable.defaults, {
        exportUrl: false,
        exportQueryParams: false,
        exportFileName: "tableExport"
    });

	var BootstrapTable = $.fn.bootstrapTable.Constructor,
		_initToolbar = BootstrapTable.prototype.initToolbar;

	BootstrapTable.prototype.initToolbar = function () {
		_initToolbar.apply(this, Array.prototype.slice.apply(arguments));
		
		if (this.options.showExport) {
			var that = this,
				$btnGroup = this.$toolbar.find('>.btn-group'),
				$export = $btnGroup.find('div.export');
				
			if($export.length) {
				var $menu = $export.find('.dropdown-menu');
				
				var $li = $menu.find('li')

				$li.off('click');

                $li.on('click', function (event) {
                    //画面のデータ、選んだ出力形式を取得
                    var data = that.options.exportQueryParams(that.$el,this.dataset.type);

                    var xhr = new XMLHttpRequest();
                    xhr.open('POST', that.options.exportUrl);
                    xhr.responseType = 'blob';
                    xhr.setRequestHeader("content-type", "application/json");

                    xhr.onloadend = function () {
                        if (xhr.response.type.indexOf("html") != -1) {
                            setTimeout(function () {
                                var fr = new FileReader()
                                fr.onload = function (e) {
                                    document.write(fr.result);
                                };
                                fr.readAsText(xhr.response);
                            }, 0);
                            return;
                        }
                        var date = new Date();
                        //ファイル名設定
                        var postfix = "_" + date.getFullYear() + ('0' + (date.getMonth() + 1)).slice(-2) + ('0' + date.getDate()).slice(-2)
                            + ('0' + date.getHours()).slice(-2) + ('0' + date.getMinutes()).slice(-2);
                        if (this.response.type == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                            //excel形式での拡張子設定
                            postfix = postfix + ".xlsx";
                        } else {
                            //CSV形式での拡張子設定
                            postfix = postfix + ".csv";
                        }
                        
                        window.navigator.msSaveBlob(xhr.response, that.options.exportFileName + postfix);
                    }
                    xhr.send(JSON.stringify(data));
				});
			}
		}
	};
})(jQuery);
