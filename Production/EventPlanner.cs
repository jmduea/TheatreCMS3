using Microsoft.AspNet.Identity.EntityFramework;
using System;
using TheatreCMS3.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace TheatreCMS3.Areas.Prod.Models
{
    // EventPlanner class inherits from ApplicationUser
    public class EventPlanner : ApplicationUser
    {
        // The PlannerStartDate property stores when the event planner started at the position
        public DateTime PlannerStartDate { get; set; }

        public static void SeedEventPlanners(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Create the EventPlanner role if it doesn't already exist
            if (!roleManager.RoleExists("EventPlanner"))
            {
                var role = new IdentityRole();
                role.Name = "EventPlanner";
                roleManager.Create(role);
            }

            // Create a new EventPlanner user
            var eventPlanner = new EventPlanner()
            {
                UserName = "eventplanner@example.com",
                Email = "eventplanner@example.com",
                PlannerStartDate = DateTime.Now
            };

            // Add the new EventPlanner user to the database and set a password
            var result = userManager.Create(eventPlanner, "EventPlanner123!");

            // If the EventPlanner user was sucessfuly created, assign the EventPlanner role
            if (result.Succeeded)
            {
                userManager.AddToRole(eventPlanner.Id, "EventPlanner");
            }
        }
    }
}