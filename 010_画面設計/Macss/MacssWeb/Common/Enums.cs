using Sic.Common.Attribute;

namespace MacssWeb.Common
{
    public enum Actions
    {
        Index,
        Create,
        Edit,
        Delete,
    }

    public enum ButtonValues
    {
        Nothing,
        Back,
        Exec,
    }

    public enum AlertTypes
    {
        [Value("alert-success")]
        Success,
        [Value("alert-danger")]
        Failure,
        [Value("alert-warning")]
        Warning,
        [Value("alert-primary")]
        Primary
    }

    public enum ProcTypes
    {
        [Value("新規")]
        New,
        [Value("更新")]
        Update,
        [Value("削除")]
        Delete
    }
}