using AutoMapper;
using cmmi.review.api.Models;
using cmmi.review.business;
using cmmi.review.entities;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace cmmi.review.api.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        private readonly IUserServices _userServices;

        public UserController()
        {
            _userServices = new UserServices();
        }

        [HttpGet]
        [Route("get/all")]
        public HttpResponseMessage GetAllUsers()
        {
            var users = _userServices.GetAllUsers();
            if (users != null)
            {
                var userEntities = users.ToList();
                if (userEntities.Any())
                {
                    // Map this to a model so we are only sending certain info to the requestor.  Password Hash and Salt not sent to requestor for security reasons.
                    Mapper.Initialize(cfg => cfg.CreateMap<UserEntity, UserModel>());
                    var userModel = Mapper.Map<List<UserEntity>, List<UserModel>>(userEntities);
                    return Request.CreateResponse(HttpStatusCode.OK, userModel);
                }
            }

            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Users not found.");
        }

        [HttpGet]
        [Route("get/{userId}")]
        public HttpResponseMessage GetUserById(int userId)
        {
            var user = _userServices.GetUserById(userId);
            if (user != null)
            {
                // Map this to a model so we are only sending certain info to the requestor.  Password Hash and Salt not sent to requestor for security reasons.
                Mapper.Initialize(cfg => cfg.CreateMap<UserEntity, UserModel>());
                var userModel = Mapper.Map<UserEntity, UserModel>(user);
                return Request.CreateResponse(HttpStatusCode.OK, userModel);
            }

            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User not found.");
        }

        [HttpPost]
        [Route("create")]
        public int CreateUser(UserModel user)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<UserModel, UserEntity>());
            var userEntity = Mapper.Map<UserModel, UserEntity>(user);

            return _userServices.CreateUser(userEntity, user.Password ?? "");
        }

        [HttpPut]
        [Route("update")]
        public bool UpdateUser(UserModel user)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<UserModel, UserEntity>());
            var userEntity = Mapper.Map<UserModel, UserEntity>(user);

            return _userServices.UpdateUser(userEntity.Id, userEntity, user.Password ?? "");
        }

        [HttpDelete]
        [Route("delete/{userId}")]
        public bool DeleteUser(int userId)
        {
            return _userServices.DeleteUser(userId);
        }
    }
}
