// Setup basic express server
var express = require('express');
var app = express();
var server = require('http').createServer(app);
var io = require('socket.io')(server);
var cors = require('cors')
var port = process.env.PORT || 3000;
var random = require('random-js')(); // uses the nativeMath engin;

server.listen(port, function () {
  console.log('Server listening at port %d', port);
});

app.use(cors());
app.use(express.static(__dirname + '/public'));

var shortid = require('shortid');
var players = [];

io.on('connection', (socket) => {
    var thisPlayerId = shortid.generate();
   console.log('client connected with id: ', thisPlayerId);
    
    socket.on('login', (data) => {
        console.log('client logged in');
        var player = {
            id: thisPlayerId,
            x: 0,
            y: 0,
            name: data.name,
            team: data.team,
            flagX: random.integer(-40, 40),
            flagY: random.integer(-40, 40),
        };

        players[thisPlayerId] = player;
        console.log('client logged in, broadcasting spawn: ', JSON.stringify(player));

        socket.emit('register', player);
        socket.broadcast.emit('spawn', player);
        socket.broadcast.emit('requestPosition');

        for (var playerId in players) {
            if (playerId == thisPlayerId) {
                continue;
            }
            socket.emit('spawn', players[playerId]);
            console.log('sending spawn to new player for id: ', players[playerId]);

        }
    });

    socket.on('move', (data) => {
        data.id = thisPlayerId;
        console.log('client moved', JSON.stringify(data));
        
        //var player = players.filter((item, thisPlayerId)=>{return item.id == thisPlayerId; });
        //player.x = data.x;
        //player.y = data.y;
        socket.broadcast.emit('move', data);
    });

    socket.on('follow', (data) => {
        console.log('follow request', data);
        data.id = thisPlayerId;
        socket.broadcast.emit('follow', data);
    });


    socket.on('updatePosition', (data) => {
        console.log('update position', data);
        data.id = thisPlayerId;
        socket.broadcast.emit('updatePosition', data);
    });

    socket.on('disconnect', () => {
        console.log('client id ', thisPlayerId, ' disconnected');
        delete players[thisPlayerId];
        socket.broadcast.emit('disconnected', { id: thisPlayerId });
    });
})