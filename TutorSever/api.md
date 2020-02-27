# API说明

## Auth

调用方法:
__GET__ : Auth?user={username}&pwd={hmac}&type={type}

参数说明:
__HMAC__ : 密钥散列消息认证码

返回结果:
若账号密码正确返回一个 __Token__
若登录不正确返回 __false__

## IsFirstLogin

调用方法:
__GET__ : FirstLogin?user={UserName}&token={Token}

参数说明:
__UserName__ : 待查询的用户账号

返回结果:
__true__ : 表示此用户为第一次登陆
返回 __false__ 的值表示此用户的非第一次登陆

## UpdatePassword

调用方法:
__GET__ : UpdatePassword?user={UserName}&token={Token}&newpsw={NewPassword}

返回结果:
__true__ : 密码更新成功
返回 __false__ 密码更新失败

## Login

调用方法:
__GET__ : Login?user={username}&token={Token}

返回结果:
Token验证通过后返回相应页面，否则返回 __403 Forbidden__

## GetInformation

调用方法:
__GET__ : Inf?user={UserName}&token={Token}

## UpdateInformation

## GetTutorInformation

调用方法:
__GET__ : TutorInf?user={UserName}&token={Token}&type={TuturType}

## UploadPic

## DownloadPic
