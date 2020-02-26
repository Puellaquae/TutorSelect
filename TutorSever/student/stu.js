var app = new Vue({
    el: "#app",
    data() {
        return {
            token: '',
            username: '',
            inf:{
                name:'李四',
                id:'20200202',
                major:'计算机科学与技术',
                college:'计算机学院',
                selfintro:"Dominae et Viri, I'm Li-Si, Li-Si is me.",
                choose:'本科导师'
            }
        };
    },
    mounted() {
        this.username = sessionStorage.getItem('user');
        this.token = sessionStorage.getItem('token');
        fetch('../Inf?user=' + this.username + '&token=' + this.token).then(x => x.json()).then(x => this.inf = x);
        if(this.inf.choose==''){
            document.getElementById('choose').style.display='flex';
        }
    },
});

function choose(num){
    if(confirm('请确认：你的选择为：'+num)){
        app.inf.choose=num;
        document.getElementById('choose').style.display='none';
    }
}

function chooseTutor(){
    document.getElementById('choose').style.display='flex';
}

function infCheck(){
    //fetch
    document.getElementById('app').style.display='none';
    document.getElementById('tutor').style.display='block';
}

function change(){
    document.getElementById('submitbtn').innerHTML='确认无误并提交更改';
}

var tutor = new Vue({
    el:'#tutor',
    data(){
        return {
            tutors:[
                {
                    name:'张三',
                    college:'计算机学院',
                    field:'人工智能',
                    position:'教授',
                    achievement:'2020 Turing Award\r\nApplication of Convolutional Neural Networks in Multilateral Games with Asymmetric Information',
                    intro:''
                },
                {
                    name:'赵六',
                    college:'外国语学院',
                    field:'线性文字A',
                    position:'教授',
                    achievement:'A Syntactic Relation Between Linear A and Meadow Mari',
                    intro:''
                }
            ]
        }
    }
});

function Add(t){
    t.style.border='1px black solid';
}