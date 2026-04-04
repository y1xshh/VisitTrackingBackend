using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitTracking.Application.Helper
{
    public static class AdminRegEmailTemplate
    {
       public static string build(string AdminName,string AdminEmail,string AdminPassword)
        {
            string template = @"<html>
                                <body>
                                    <h2>Admin Registration Successful</h2>
                                    <p>Dear {AdminName},</p>
                                    <p>Your registration as an admin has been successful. Below are your login details:</p>
                                    <ul>
                                        <li><strong>Email:</strong> {AdminEmail}</li>
                                        <li><strong>Password:</strong> {AdminPassword}</li>
                                    </ul>
                                    <p>Please keep this information secure and do not share it with anyone.</p>
                                    <p>Best regards,<br/>Visit Tracking Team</p>
                                </body>
                            </html>";
            return template;
        }

        internal static string build(string? fullName, string? email)
        {
            throw new NotImplementedException();
        }
    }
}
