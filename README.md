jdcapi
======

Juts-Dice Custom Application Programming Interface


This is an API to use with http://just-dice.com

donations:

bitcoin: 13tCUHA7TDeHMuRAta1wJQ7UchDep3uT2z

clam: x8NNzdwgNzuCqhQuPkmHvGHNTzonhvrqZd


It is currently only available in .net, but it will be ported to mono soon, so it can be used on linux and mac as well as windows.

There is still a lot of work to be done, some features are not working, others are not working optimally yet. Please take care when using this for now.

This is an event driven api, all messages from the server triggers an event, to send data to the server, use the approriate method.

Quick example for code to subscribe to the chat and send a chat message.

public static void main()
{
//create the instance
jdInstnce Instance = new Instance();

//register events handler to get the old chat messages when the bot logs in successfully
Instance.onOldChat+=OldChat;

//register event handler for new chat messages
Instance.onChat+=onChat;

//Connect to the site. 3 overloaded methods here
//1: Connect to Just dice or doge dice, creates a new account
//params: bool dogedice - if set to true, it connects to doge-dice.com, if false, connects to just-dice.com
Instance.Connect(false);

//2:Connects and immediately logs in using your secret hash. Do not use this is account is set to use a username and password.
//params:bool dogedice - if set to true, it connects to doge-dice.com, if false, connects to just-dice.com
// string SecretHash - your secret hash, found on the account tab, or the long string of characters behind the url when connecting
//Instance.Connect(false, "your secret hash");

//2:Connects and immediately logs in using your secret hash. Do not use this is account is set to use a username and password.
//params:bool dogedice - if set to true, it connects to doge-dice.com, if false, connects to just-dice.com
//rest are self explanitory. Have not tested login with the GACode
//Instance.Connect(false, "uesrname", "password","Google Auth Code");

//the connect method is syncronous, I still need to add something so you can actually know the bot is connected, currently the best way to test it would be to 
//make sure Instance.UID is not empty.

//send a chat message
Instance.Chat("Your chat message here");

//place a bet, params: chance, bet size, hi/lo (hi=true)
Instance.Bet(49.5,0.001,true);
}

//Chat class has properties: string Message, string User, string UID, DateTime Date
public static void OldChat(Chat chat)
{
Console.WriteLine(Chat.Message);
}

public static void onChat(Chat chat)
{
Console.WriteLine(Chat.Message);
}
