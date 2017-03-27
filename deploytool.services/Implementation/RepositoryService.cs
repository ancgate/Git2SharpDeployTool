using System;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using System.Text;
using DeploymentTool.Services.Model;
using System.IO;

namespace deploytool.services.Implementation
{
    public class RepositoryService
    {       
        public ResultModel CloneRepository(string targetDir, string reposUrl, UsernamePasswordCredentials credentials,
            string branchName = "")
        {
            var strResult = new StringBuilder();
            strResult.Append(string.Format("Cloning {0} repository to: {1}", reposUrl, targetDir));

            try
            {
                CredentialsHandler credHandler = (url, user, cred) => credentials;

                var options = new CloneOptions
                {
                    IsBare = false,
                    CredentialsProvider = credHandler, 
                    BranchName = string.IsNullOrEmpty(branchName)? "master" : branchName
                };

                Repository.Clone(reposUrl, targetDir, options);
                strResult.Append(string.Format("Repository clone success to: {0}", targetDir));             

            }
            catch(Exception ex)
            {
                return new ResultModel
                {
                    IsSuccess = true,
                    Logs = strResult.ToString(),
                    ErrorMessage = ex.Message
                };
            }
            finally
            {
                               
            }

           return new ResultModel
                {
                    IsSuccess = true,
                    Logs = strResult.ToString()
                };
        }

        public ResultModel PullRepository(string targetDir, UsernamePasswordCredentials credentials,string branchName = "")
        {
            var strResult = new StringBuilder();
            strResult.Append(string.Format("Pulling existing repository {0}", targetDir));

            try
            {
                using (var repo = new Repository(targetDir))
                {
                    var options = new PullOptions
                    {
                        FetchOptions = new FetchOptions
                        {
                            CredentialsProvider = (url, usernameFromUrl, types) => credentials
                        }
                    };

                    var signature = new Signature(credentials.Username, credentials.Password, new DateTimeOffset(DateTime.Now));

                    Commands.Pull(repo, signature, options);
                }

                strResult.Append(string.Format("Repository pulled success to: {0}", targetDir));
                return new ResultModel
                {
                    IsSuccess = true,
                    Logs = strResult.ToString()
                };
            }
            catch (Exception ex)
            {
                return new ResultModel
                {
                    IsSuccess = true,
                    Logs = strResult.ToString(),
                    ErrorMessage = ex.Message
                };
            }
        }

        public ResultModel PullOrCloneRepository(string targetDir, string reposUrl, UsernamePasswordCredentials credentials)
        {
            if (string.IsNullOrWhiteSpace(targetDir) || string.IsNullOrEmpty(reposUrl))
            {
                return new ResultModel
                {
                    IsSuccess = false,
                    ErrorMessage = "Missing targetDir or repositoryUrl"
                };
            }

            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            return Repository.IsValid(targetDir)? 
                PullRepository(targetDir, credentials):            
                CloneRepository(targetDir, reposUrl, credentials);
               
        }

    }
}
