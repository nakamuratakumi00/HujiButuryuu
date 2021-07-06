using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Macss.ViewModels;

namespace Macss.Controllers
{
    public static class SessionExtensions
    {

        public static class Field
        {
            public const string UserID = "userid";
            public const string Password = "password";
            public const string UserName = "username";
            public const string Menu = "menu";
            public const string Processing = "processing";
            public const string ViewModel = "viewModel";
            // グループコード
            public const string GroupCd = "GroupCd";

            public const string Information = "Information";

        }


        public static string GetUserID(this HttpSessionStateBase session)
        {
            return session[Field.UserID] as string;
        }
        public static string GetUserID(this HttpSessionState session)
        {
            return session[Field.UserID] as string;
        }

        public static void SetUserID(this HttpSessionStateBase session, string userID)
        {
            session[Field.UserID] = userID;
        }

        public static void SetUserID(this HttpSessionState session, string userID)
        {
            session[Field.UserID] = userID;
        }

        public static string GetUserName(this HttpSessionStateBase session)
        {
            return session[Field.UserName] as string;
        }

        public static string GetUserName(this HttpSessionState session)
        {
            return session[Field.UserName] as string;
        }

        public static void SetUserName(this HttpSessionStateBase session, string userName)
        {
            session[Field.UserName] = userName;
        }

        public static void SetUserName(this HttpSessionState session, string userName)
        {
            session[Field.UserName] = userName;
        }

        public static List<MenuViewModels> GetMenu(this HttpSessionStateBase session)
        {
            return session[Field.Menu] as List<MenuViewModels>;
        }
        public static List<MenuViewModels> GetMenu(this HttpSessionState session)
        {
            return session[Field.Menu] as List<MenuViewModels>;
        }
        public static void SetMenu(this HttpSessionStateBase session, List<MenuViewModels> groups)
        {
            session[Field.Menu] = groups;
        }

        public static void SetMenu(this HttpSessionState session, List<MenuViewModels> groups)
        {
            session[Field.Menu] = groups;
        }
        public static string GetProcessingID(this HttpSessionStateBase session)
        {
            return session[Field.Processing] as string;
        }
        public static void SetProcessingID(this HttpSessionStateBase session, string processingID)
        {
            session[Field.Processing] = processingID;
        }


        public static object GetViewModel(this HttpSessionStateBase session)
        {
            return session[Field.ViewModel] as object;
        }
        public static void SetViewModel(this HttpSessionStateBase session, object viewModel)
        {
            session[Field.ViewModel] = viewModel;
        }

        public static void RemoveViewModel(this HttpSessionStateBase session)
        {
            session.Remove(Field.ViewModel);
        }


        public static string GetGroupCd(this HttpSessionStateBase session)
        {
            return session[Field.GroupCd] as string;
        }
        public static void SetGroupCd(this HttpSessionStateBase session, string GroupCd)
        {
            session[Field.GroupCd] = GroupCd;
        }

        public static void RemoveGroupCdl(this HttpSessionStateBase session)
        {
            session.Remove(Field.GroupCd);
        }

    }
}