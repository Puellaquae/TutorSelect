function checkPsw(psw) {
    var modes = 0;
    if (psw.length < 6) { //最初级别
        return modes;
    }
    if (/\d/.test(psw)) { //密码包含数字
        modes++;
    }
    if (/[a-z]/.test(psw)) { //密码包含了小写字母
        modes++;
    }
    if (/[A-Z]/.test(psw)) { //密码包含了大写字母
        modes++;
    }
    return modes;
}

var app = new Vue({
    el: '#app',
    data() {
        var checkNewPassword = (rule, value, callback) => {
            if (!value) {
                return callback(new Error('不能为空'));
            } else {
                if (checkPsw(value) < 3) {
                    return callback(new Error('密码强度不足'));
                } else {
                    callback();
                }
            }
        }
        var confirmNewPassword = (rule, value, callback) => {
            if (!value) {
                return callback(new Error('不能为空'));
            } else {
                if (value!=this.login.newpassword) {
                    return callback(new Error('两次密码不一致'));
                } else {
                    callback();
                }
            }
        }
        return {
            login: {
                username: '',
                password: '',
                usergroup: '',
                newpassword: '',
                checknewpsw:'',
                firstlogin: false,
                token: ''
            },
            rules: {
                username: [{
                    required: true,
                    message: '账号不能为空',
                    trigger: 'blur'
                }],
                password: [{
                    required: true,
                    message: '请输入密码',
                    trigger: 'blur'
                }],
                usergroup: [{
                    required: true,
                    message: '请选择',
                    trigger: 'blur'
                }],
                newpassword: [{
                    validator: checkNewPassword,
                    trigger: 'blur'
                }],
                checknewpsw: [{
                    validator: confirmNewPassword,
                    trigger: 'blur'
                }]
            }
        };
    },
    methods: {
        submitForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    let url = 'Auth?user=' + this.login.username + '&type=' + this.login.usergroup + '&pwd=' + md5(this.login.password, this.login.username);
                    fetch(url)
                        .then(
                            X => {
                                if (X.ok) {
                                    return X.text();
                                } else {
                                    this.$alert('网络错误，请重试', '警告', {
                                        confirmButtonText: '确定',
                                        type: 'error',
                                        center: true
                                    });
                                }
                            }
                        ).then(X => {
                            if (X != 'false') {
                                this.login.token = X;
                                window.sessionStorage.setItem('token',X);
                                window.sessionStorage.setItem('user',this.login.username);
                                if (this.login.usergroup == 'student') {
                                    fetch('FirstLogin?user=' + this.login.username + '&token=' + this.login.token)
                                        .then(X => X.text())
                                        .then(X => {
                                            if (X == 'true') {
                                                this.login.firstlogin = true;
                                            } else {
                                                window.location = 'Login?user=' + this.login.username + '&token=' + this.login.token;
                                            }
                                        })
                                } else {
                                    window.location = 'Login?user=' + this.login.username + '&token=' + this.login.token;
                                }
                            } else {
                                this.$alert('账号或用户名错误', '警告', {
                                    confirmButtonText: '确定',
                                    type: 'error',
                                    center: true
                                });
                            }
                        });
                } else {
                    return false;
                }
            });
        },
        updatePassword(done) {
            this.$confirm('请提交新密码')
            .then(_ => {})
            .catch(_ => {});
        },
        submitNewPsw(formName){
            this.$refs[formName].validate((valid) => {
                if (valid) {
                  fetch('/UpdatePassword?user='+this.login.username+'&token='+this.login.token+'&newpsw='+md5(this.login.newpassword,this.login.username))
                  .then(X=>X.text())
                  .then(X=>{
                    if(X=='true'){
                        this.login.firstlogin=false;
                        this.$alert('密码修改成功，请重新登陆', '通知', {
                            confirmButtonText: '确定',
                            type: 'confirm',
                            center: true
                        });
                        window.location.reload();
                    }else{
                        this.$alert('密码修改失败，请重新尝试', '错误', {
                            confirmButtonText: '确定',
                            type: 'error',
                            center: true
                        });
                        return false;
                    }
                  })
                } else {
                  return false;
                }
              });
        }
    }
});