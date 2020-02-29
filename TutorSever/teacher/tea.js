var app = new Vue({
    el: "#app",
    data() {
        return {
            token: '',
            username: '',
            inf: {
                name: '张三',
                college: '计算机学院',
                field: '人工智能',
                position: '教授',
                group:'',
                achievement: '2020 Turing Award\r\nApplication of Convolutional Neural Networks in Multilateral Games with Asymmetric Information',
                selfintro: 'Hello World!'
            }
        };
    },
    mounted() {
        this.username = sessionStorage.getItem('user');
        this.token = sessionStorage.getItem('token');
        fetch('../Inf?user=' + this.username + '&token=' + this.token).then(x => x.json()).then(x => this.inf = x);
        if (this.inf.group == '') {
            document.getElementById('choose').style.display = 'flex';
        }
    },
});

function choose(num) {
    app.inf.group = num;
    document.getElementById('choose').style.display = 'none';
}

function chooseTutor() {
    document.getElementById('choose').style.display = 'flex';
}

function infCheck() {
    //fetch
    if (app.inf.group == '本科导师') {
        sessionStorage.setItem('type', 'under');
    } else if (app.inf.group == '毕业设计导师') {
        sessionStorage.setItem('type', 'project');
    }
    document.getElementById('app').style.display = 'none';
    document.getElementById('tutor').style.display = 'grid';
    let username = sessionStorage.getItem('user');
    let token = sessionStorage.getItem('token');
    let type = sessionStorage.getItem('type');
    fetch('../TutorInf?user=' + username + '&token=' + token + '&type=' + (type)).then(x => x.json()).then(x => this.tutors = x);
    let first = app.inf.firsttutor;
    let second = app.inf.secondtutor;
    if (first != null) {
        for (let i = 0; i < tutor.tutors.length && !has; i++) {
            if (tutor.tutors[i].id == first) {
                tutor.choosedTutor.push(tutor.tutors[i]);
                tutor.tutors.splice(i, 1);
                has = true;
            }
        }
    }
    if (second != null) {
        for (let i = 0; i < tutor.tutors.length && !has; i++) {
            if (tutor.tutors[i].id == second) {
                tutor.choosedTutor.push(tutor.tutors[i]);
                tutor.tutors.splice(i, 1);
                has = true;
            }
        }
    }

}

function change() {
    document.getElementById('submitbtn').innerHTML = '确认无误并提交更改';
}

var tutor = new Vue({
    el: '#tutor',
    data() {
        return {
            choosedTutor: [],
            tutors: [{
                    name: '张三',
                    id: '',
                    college: '计算机学院',
                    field: '人工智能',
                    position: '教授',
                    achievement: '2020 Turing Award\r\nApplication of Convolutional Neural Networks in Multilateral Games with Asymmetric Information',
                    intro: 'Hello World!'
                }
            ]
        }
    },
    methods: {
        choose: function (t) {
            if (this.choosedTutor.length < 2) {
                var has = false;
                for (let i = 0; i < this.choosedTutor.length && !has; i++) {
                    if (this.choosedTutor[i] == t) {
                        has = true;
                    }
                }
                if (!has) {
                    this.choosedTutor.push(t);
                }
                has = false;
                for (let i = 0; i < this.tutors.length && !has; i++) {
                    if (this.tutors[i] == t) {
                        this.tutors.splice(i, 1);
                        has = true;
                    }
                }
            }
        },
        outchoose: function (t) {
            let has = false;
            for (let i = 0; i < this.tutors.length && !has; i++) {
                if (this.tutors[i] == t) {
                    has = true;
                }
            }
            if (!has) {
                this.tutors.push(t);
            }
            has = false;
            for (let i = 0; i < this.choosedTutor.length && !has; i++) {
                if (this.choosedTutor[i] == t) {
                    this.choosedTutor.splice(i, 1);
                    has = true;
                }
            }
        }
    }
});
var chosed = 0;

function Add(t) {
    if (t.getAttribute('choose') == 'false') {
        if (chosed == 0) {
            t.setAttribute('choose', 'first');
            chosed++;
        } else if (chosed == 1) {
            t.setAttribute('choose', 'second');
            chosed++;
        }
    } else {
        t.setAttribute('choose', 'false');
        chosed--;
    }
}

function updownpic() {
    document.getElementById('fileElem').click();
}

function pickPic(file) {
    let reader;
    if (file) {
        reader = new FileReader();
        reader.readAsDataURL(file);
    }
    reader.onload = function (e) {
        document.getElementById('yourpic').src = e.target.result;
        fetch();
    }
}