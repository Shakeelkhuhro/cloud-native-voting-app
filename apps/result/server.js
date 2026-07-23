const express=require("express");
const path=require("path");
const http=require("http");
const socketio=require("socket.io");
const {Pool}=require("pg");

const app=express();
const server=http.createServer(app);
const io=socketio(server);

const pool=new Pool({
    host:process.env.DB_HOST||"postgresql",
    port:process.env.DB_PORT||5432,
    database:process.env.DB_NAME||"postgres",
    user:process.env.DB_USERNAME||process.env.POSTGRES_USER||"postgres",
    password:process.env.DB_PASSWORD||process.env.POSTGRES_PASSWORD||"postgres"
});

function sendVotes(){
    pool.query(
        "SELECT vote,COUNT(id) count FROM votes GROUP BY vote",
        (err,res)=>{
            if(!err){
                let votes={a:0,b:0};
                res.rows.forEach(r=>votes[r.vote]=parseInt(r.count));
                io.emit("scores",JSON.stringify(votes));
            }
            setTimeout(sendVotes,1000);
        }
    );
}

pool.connect(err=>{
    if(err){
        console.log("Waiting for db");
        setTimeout(()=>process.exit(1),2000);
    }else{
        console.log("Connected");
        sendVotes();
    }
});

app.use(express.static(path.join(__dirname,"views")));

app.get("/",(req,res)=>{
    res.sendFile(path.join(__dirname,"views","index.html"));
});

server.listen(process.env.PORT||4000);
