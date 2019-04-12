var SchedulerMgr = function () {
    var exports = {};
    exports.options = {};
    exports.options.ItemId = "";
    exports.options.ItemName = "";
    //任务状态
    exports.options.TaskStatusTypes = {
        0: "label label-default",
        1: "label label-info",
        2: "label label-warning",
        3: "label label-danger",
        4: "label label-success"
    };
    //任务类型
    exports.options.JobTypes = {
        0: "label label-success",
        1: "label label-info"
    }
    //初始化数据
    exports.initGridPage = function () {

        //初始化
        $(window).resize(function (e) {
            window.setTimeout(function () {
                $('#gridTable').setGridWidth(($('.gridPanel').width()));
                $("#gridTable").setGridHeight($.fn.getGridHeight(true));

            }, 100);
            e.stopPropagation();
        });

        //查询条件
        $("#taskStatusCondition .dropdown-menu li").click(function () {
            var text = $(this).find('a').html();
            var value = $(this).find('a').attr('data-value');
            $("#taskStatusCondition .dropdown-text").html(text).attr('data-value', value);
        });
        $("#taskTypeCondition .dropdown-menu li").click(function () {
            var text = $(this).find('a').html();
            var value = $(this).find('a').attr('data-value');
            $("#taskTypeCondition .dropdown-text").html(text).attr('data-value', value);
        });

        //查询回车
        $('#btn_Search').bind('keypress', function (event) {
            if (event.keyCode == "13") {
                $('.lr-search').trigger("click");
            }
        });

        //注册事件
        $(".titlePanel").on("click", ".lr-replace,.lr-add,.lr-edit,.lr-search,.lr-viewlog,.lr-start,.lr-stop,.lr-delete", function () {
            var $this = $(this);
            //刷新
            if ($this.hasClass('lr-replace')) {
                reload();
            }
            else if ($this.hasClass('lr-search')) {
                exports.SearchEvent();
            }
            else if ($this.hasClass('lr-add')) {
                exports.btnAdd();
            }
            else if ($this.hasClass('lr-removelog')) {
                exports.btnRemoveLog();
            }
            else if ($this.hasClass('lr-viewlog')) {
                exports.btnViewTaskLog();
            }
            else if ($this.hasClass('lr-start')) {
                exports.btnEnabled();
            }
            else if ($this.hasClass('lr-stop')) {
                exports.btnDiabled();
            }
            else if ($this.hasClass("lr-delete")) {
                exports.btnDelete();
            };
        });

    };
    //编辑
    exports.btnEdit = function () {
        var keyValue = $("#gridTable").jqGridRowValue("Id");
        var taskName = $("#gridTable").jqGridRowValue("TaskName");
        if (checkedRow(keyValue)) {
            $.fn.modalOpen({
                id: "jobDetailForm",
                title: '编辑【' + taskName + '】任务',
                url: '/pages/supermgr/JobDetailForm.html?keyValue=' + keyValue,
                width: "750px",
                height: "550px",
                callBack: function (iframeId) {
                    top.frames[iframeId].AcceptClick();
                }
            });
        }
    }
    //添加
    exports.btnAdd = function () {
        $.fn.modalOpen({
            id: "jobDetailForm",
            title: '添加任务',
            url: '/pages/supermgr/JobDetailForm.html',
            width: "750px",
            height: "550px",
            callBack: function (iframeId) {
                top.frames[iframeId].AcceptClick();
            }
        });
    }

    //查看任务日志
    exports.btnViewTaskLog = function () {
        var keyValue = $("#gridTable").jqGridRowValue("Id");
        var jobName = $("#gridTable").jqGridRowValue("TaskName");
        if (checkedRow(keyValue)) {
            $.fn.modalOpen({
                id: "Form",
                title: '查看【' + jobName + '】任务日志',
                url: '/pages/supermgr/ViewJobLog.html?keyValue=' + keyValue,
                width: "900px",
                height: "650px",
                callBack: function (iframeId) {
                    // top.frames[iframeId].AcceptClick();
                }
            });
        }
    }

    //加载Grid
    exports.loadGrid = function () {
        var selectedRowIndex = 0;
        exports.options.$gridTable = $("#gridTable");
        exports.options.$gridTable.jqGrid({
            datatype: "json",
            mtype: "POST",
            url: "/UpgradePlan/ServerInfo",
            height: $.fn.getGridHeight(true),
            autowidth: true,
            multiselect: true,
            colModel: [
                { label: "主键", name: "UUID", hidden: true },
                { label: "序号", name: "SEQNUM", index: "SEQNUM", width: 50, align: "center" },
                { label: "部署点名称", name: "SERVERNAME", index: "SERVERNAME", width: 150, align: "left" },
                { label: "内网IP", name: "INTERNALIP", index: "INTERNALIP", width: 150, align: "left" },
                { label: "内网端口", name: "PORT", index: "PORT", width: 100, align: "center" },
                { label: "内网URL", name: "URLADDR", index: "URLADDR", width: 150, align: "left" },
                { label: "外网IP", name: "EXTERNALIP", index: "EXTERNALIP", width: 150, align: "left" },
                { label: "外网端口", name: "EXTERNALPORT", index: "EXTERNALPORT", width: 100, align: "center" },
                { label: "外网URL", name: "EXTERNALURL", index: "EXTERNALURL", width: 150, align: "left" },
                { label: "国家或地区", name: "COUNTRYADDR", index: "COUNTRYADDR", width: 100, align: "center" },
                { label: "部署点类型", name: "SERVERTYPE", index: "SERVERTYPE", width: 100, align: "center" },
                { label: "操作系统", name: "SERVEROS", index: "SERVEROS", width: 100, align: "center" },
            ],
            pager: "#gridPager",
            sortname: '',
            rowList: [50, 100],
            rowNum: "50",
            sortorder: "desc",
            onSelectRow: function () {
                $("#txtUpgradeList").val($("#" + this.id).jqGridRowValue('SERVERNAME'));
                $("#upgradeIds").val($("#" + this.id).jqGridRowValue('UUID'));
            },
            onSelectAll: function () {
                $("#txtUpgradeList").val($("#" + this.id).jqGridRowValue('SERVERNAME'));
                $("#upgradeIds").val($("#" + this.id).jqGridRowValue('UUID'));
            },
            gridComplete: function () {
                //$("#" + this.id).setSelection(selectedRowIndex, false);
            },
            loadError: function (xhr, status, error) {
                switch (error) {
                    case "Unauthorized":
                        top.location.href = "/account/login";
                        break;
                    default:
                        $.fn.modalMsg(error, "error");
                        break;
                }
            }
        });
        exports.SearchEvent(0);
    }
    //查询表格函数
    exports.SearchEvent = function () {
        var name = $("#txtName").val();
        $("#gridTable").jqGrid('setGridParam', {
            url: "/UpgradePlan/ServerInfo",
            postData: { name: name },
            page: 1
        }).trigger('reloadGrid');
    }
    //验证：项目值、项目名 不能重复
    exports.OverrideExistField = function (id, url) {
        $.fn.existField(id, url, { itemId: itemId });
    }

    //删除
    exports.btnDelete = function (keyValue) {
        if (keyValue == undefined) {
            keyValue = $("#gridTable").jqGridRowValue("Id");
        }
        if (checkedRow(keyValue)) {
            $.fn.confirmAjax({
                msg: "注：您确定要【删除】该定时任务么？该删除操作会级联删除任务日志，请谨慎操作！",
                url: "/SysMgr/SchedulerMgr/DeleteJobStatus",
                param: { keyValue: keyValue },
                success: function (data) {
                    $("#gridTable").trigger("reloadGrid");
                }
            });
        }
    }
    //启用
    exports.btnEnabled = function (keyValue) {
        if (keyValue == undefined) {
            keyValue = $("#gridTable").jqGridRowValue("Id");
        }
        if (checkedRow(keyValue)) {
            $.fn.confirmAjax({
                msg: "注：您确定要【启动】该定时任务么？",
                url: "/SysMgr/SchedulerMgr/ManageJobStatus",
                param: { keyValue: keyValue, jobStatus: 1 },
                success: function (data) {
                    $("#gridTable").trigger("reloadGrid");
                }
            });
        }
    }
    //禁用
    exports.btnDiabled = function (keyValue) {
        if (keyValue == undefined) {
            keyValue = $("#gridTable").jqGridRowValue("Id");
        }
        if (checkedRow(keyValue)) {
            $.fn.confirmAjax({
                msg: "注：您确定要【停止】该定时任务么？",
                url: "/SysMgr/SchedulerMgr/ManageJobStatus",
                param: { keyValue: keyValue, jobStatus: 2 },
                success: function (data) {
                    $("#gridTable").trigger("reloadGrid");
                }
            });
        }
    }
    //保存表单
    exports.AcceptClick = function () {
        if (!$('#form1').Validform()) {
            return false;
        }
        var postData = $("#form1").GetWebControls(exports.options.KeyValue);
        $.fn.submitForm({
            url: "/SysMgr/SchedulerMgr/Save?keyValue=" + exports.options.KeyValue,
            param: postData,
            loading: "正在保存数据...",
            success: function () {
                $.currentIframe().$("#gridTable").resetSelection();
                $.currentIframe().$("#gridTable").trigger("reloadGrid");
            }
        });
    };
    //初始化表单
    exports.initFormControl = function (readonly) {
        exports.options.KeyValue = $.fn.request("keyValue");
        $("#TaskType").ComboBox({
            description: "==请选择=="
        });
        //获取表单
        if (!!exports.options.KeyValue) {
            $.fn.setForm({
                url: "/SysMgr/SchedulerMgr/GetJobDetailEntity",
                param: { keyValue: exports.options.KeyValue },
                success: function (data) {
                    $("#form1").SetWebControls(data);
                    if (readonly) {

                        // $("#form1").find('.form-control,.ui-select,input,textarea').attr('disabled', 'disabled');
                        $("#form1").find("#BeginTime,#AssemblyDll,#Class,#TaskType,#CronExpressionString").attr("disabled", 'disabled');
                        // $("#TaskName,#TaskGroup,#TaskType")
                    }
                }
            });
        }
    }

    return exports;
};