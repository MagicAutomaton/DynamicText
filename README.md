<h1>DynamicText</h1>
This project is a custom text component in Unity.

Many thanks to [@markv12](https://github.com/markv12), I watched his [vedio](https://www.youtube.com/watch?v=So8DpNh3XOE) and learned a lot from [his code](https://github.com/markv12/VertexTextAnimationDemo).

The first edition of this system has almost the same functions as the project mentioned above, but is more stable (maybe).

<h2>Usage</h2>

You can find TextSender.cs in the project. I use a Start() to call SendTextToTMP().

This function is used to send text to Text Mesh Pro. You can call this function wherever you need to.

<image src="https://raw.githubusercontent.com/MagicAutomaton/MyImages/main/DynamicText/Start.png">

<h2>Tags</h2>

Here are four tags you can use in your text:

<h3>&lt;pause&gt;</h3>
Use this to make the text pause for a period of time.

Format:&lt;pause <i>time</i>&gt;

Example:

<image src="https://raw.githubusercontent.com/MagicAutomaton/MyImages/main/DynamicText/pause.gif">

<h3>&lt;speed&gt;</h3>

Use this to change the speed at which text apears.(Default value:8)

Format:&lt;speed <i>speed</i>&gt;

Example:

<image src="https://raw.githubusercontent.com/MagicAutomaton/MyImages/main/DynamicText/speed.gif">

<h3>&lt;wave&gt;</h3>

Format:&lt;wave <i>intensity</i>&gt;&lt;/wave&gt;

<i>If a wave tag is nested inside another wave tag, their effects will be added. </i>

Example:

<image src="https://raw.githubusercontent.com/MagicAutomaton/MyImages/main/DynamicText/wave.gif">

<h3>&lt;shake&gt;</h3>

Format:&lt;shake <i>intensity</i>&gt;&lt;/shake&gt;

<i>If a shake tag is nested inside another shake tag, their effects will be added. </i>

Example:

<image src="https://raw.githubusercontent.com/MagicAutomaton/MyImages/main/DynamicText/shake.gif">

<h3>&lt;colorful&gt;</h3>

Format:&lt;colorful&gt;&lt;/colorful&gt;

<i>If a colorful tag is nested inside another colorful tag, nothing special will happen.</i>

Example:

<image src="https://raw.githubusercontent.com/MagicAutomaton/MyImages/main/DynamicText/colorful.gif">

<h3>&lt;alpha&gt;</h3>

Format:&lt;alpha <i>topAlpha</i> <i>bottomAlpha</i>&gt;&lt;/alpha&gt;
  
<i>If an alpha tag is nested inside another alpha tag, the inner one will take effect.</i>
  
Example:

<image src="https://raw.githubusercontent.com/MagicAutomaton/MyImages/main/DynamicText/alpha.gif">
