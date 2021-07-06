using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Macss.Database.Entity;
using MacssWeb.Models.Context;

namespace MacssWeb.Controllers
{
    public class AccountMastersController : Controller
    {
        private MacssWebDbContext db = new MacssWebDbContext();

        // GET: AccountMasters
        public ActionResult Index()
        {
            return View(db.m_account.ToList());
        }

        // GET: AccountMasters/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountMaster accountMaster = db.m_account.Find(id);
            if (accountMaster == null)
            {
                return HttpNotFound();
            }
            return View(accountMaster);
        }

        // GET: AccountMasters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountMasters/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "account_id,account_name,account_name_kana,account_password,group_cd,login_count,login_failure_count,last_login_date,delete_flg,create_id,create_date,update_id,update_date,bumon_cd,basyo_cd,account_password_date,sdek12,ctlfl1")] AccountMaster accountMaster)
        {
            if (ModelState.IsValid)
            {
                db.m_account.Add(accountMaster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(accountMaster);
        }

        // GET: AccountMasters/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountMaster accountMaster = db.m_account.Find(id);
            if (accountMaster == null)
            {
                return HttpNotFound();
            }
            return View(accountMaster);
        }

        // POST: AccountMasters/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "account_id,account_name,account_name_kana,account_password,group_cd,login_count,login_failure_count,last_login_date,delete_flg,create_id,create_date,update_id,update_date,bumon_cd,basyo_cd,account_password_date,sdek12,ctlfl1")] AccountMaster accountMaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(accountMaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(accountMaster);
        }

        // GET: AccountMasters/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountMaster accountMaster = db.m_account.Find(id);
            if (accountMaster == null)
            {
                return HttpNotFound();
            }
            return View(accountMaster);
        }

        // POST: AccountMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AccountMaster accountMaster = db.m_account.Find(id);
            db.m_account.Remove(accountMaster);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
