var app = new Vue({
    el: "#app",
    data() {
        return {
            token: '',
            username: '',
            inf: {
                name: '李四',
                id: '20200202',
                major: '计算机科学与技术',
                college: '计算机学院',
                selfintro: "Dominae et Viri, I'm Li-Si, Li-Si is me.",
                choose: '本科导师',
                firsttutor: '',
                secondtutor: ''
            }
        };
    },
    mounted() {
        this.username = sessionStorage.getItem('user');
        this.token = sessionStorage.getItem('token');
        fetch('../Inf?user=' + this.username + '&token=' + this.token).then(x => x.json()).then(x => this.inf = x);
        if (this.inf.choose == '') {
            document.getElementById('choose').style.display = 'flex';
        }
    },
});

function choose(num) {
    app.inf.choose = num;
    document.getElementById('choose').style.display = 'none';
}

function chooseTutor() {
    document.getElementById('choose').style.display = 'flex';
}

function infCheck() {
    //fetch
    if (this.choose == '本科导师') {
        sessionStorage.setItem('type','under');
    } else if (this.choose == '毕业设计导师') {
        sessionStorage.setItem('type','project');
    }
    document.getElementById('app').style.display = 'none';
    document.getElementById('tutor').style.display = 'grid';
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
                    id:'',
                    college: '计算机学院',
                    field: '人工智能',
                    position: '教授',
                    achievement: '2020 Turing Award\r\nApplication of Convolutional Neural Networks in Multilateral Games with Asymmetric Information',
                    intro: 'Hello World!'
                },
                {
                    name: '赵六',
                    id:'',
                    college: '外国语学院',
                    field: '线性文字A',
                    position: '教授',
                    achievement: 'A Syntactic Relation Between Linear A and Meadow Mari',
                    intro: ''
                }
            ]
        }
    },
    methods: {
        choose: function (t) {
            if (this.choosedTutor.length < 2) {
                let has = false;
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
    },
    mounted() {
        let username = sessionStorage.getItem('user');
        let token = sessionStorage.getItem('token');
        let type = sessionStorage.getItem('type');
        fetch('../TutorInf?user=' + username + '&token=' + token + '&type=' + (type)).then(x => x.json()).then(x => this.tutors = x);
        let first=app.inf.firsttutor;
        let second=app.inf.secondtutor;
        if(first!=null){
            for (let i = 0; i < this.tutors.length && !has; i++) {
                if (this.tutors[i].id == first) {
                    this.choosedTutor.push(this.tutors[i]);
                    this.tutors.splice(i, 1);
                    has = true;
                }
            }
        }
        if(second!=null){
            for (let i = 0; i < this.tutors.length && !has; i++) {
                if (this.tutors[i].id == second) {
                    this.choosedTutor.push(this.tutors[i]);
                    this.tutors.splice(i, 1);
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