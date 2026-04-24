namespace VisitTracking.Application.Helper
{
    public static class EmpRegEmailTemplate
    {
        public static string Build(string empName, string empEmail, string empPassword)
        {
            string template = @"<html>
                                <body style='font-family: Arial;'>
                                    <h2>Employee Registration Successful</h2>
                                    
                                    <p>Dear {EmpName},</p>
                                    
                                    <p>Your account has been created successfully. Below are your login details:</p>
                                    
                                    <ul>
                                        <li><strong>Email:</strong> {EmpEmail}</li>
                                        <li><strong>Password:</strong> {EmpPassword}</li>
                                    </ul>
                                    
                                    <p style='color:red;'><strong>Please change your password after first login.</strong></p>
                                    
                                    <p>Best regards,<br/>Visit Tracking Team</p>
                                </body>
                            </html>";

            template = template.Replace("{EmpName}", empName);
            template = template.Replace("{EmpEmail}", empEmail);
            template = template.Replace("{EmpPassword}", empPassword);

            return template;
        }
    }
}