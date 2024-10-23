using Drogueria_Elcafetero.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Drogueria_Elcafetero.Permisos
{
    public class PermisosRolAttribute : ActionFilterAttribute
    {
        private readonly Rol id_rol;

        public  PermisosRolAttribute(Rol _id_rol)
        {
            id_rol = _id_rol;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var session = context.HttpContext.Session.GetString("Empleado");

            if (session != null)
            {
                var employee = JsonConvert.DeserializeObject<Employees>(session);

                if (employee.id_Rol != id_rol)
                {
                    context.Result = new RedirectToActionResult("SinPermiso","Home",null);
                }
            }
        }


    }
}
