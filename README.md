# DynamicText
This project is a custom text component in Unity.

Many thanks to [@markv12](https://github.com/markv12), I watched his [vedio](https://www.youtube.com/watch?v=So8DpNh3XOE) and learned a lot from [his code](https://github.com/markv12/VertexTextAnimationDemo).

The first edition of this system has almost the same functions as the project mentioned above, but is more stable.

Here are four tags you can use in your text:

<h1>&lt;pause&gt;</h1>
Use this to make the text pause for a period of time.

Format:&lt;pause <i>time</i>&gt;

Example:

<image src="https://raw.githubusercontent.com/MagicAutomaton/MyImages/main/DynamicText/pause.gif">

<h1>&lt;speed&gt;</h1>

Use this to change the speed at which text apears.(Default value:8)

Format:&lt;speed <i>speed</i>&gt;

Example:

<image src="https://raw.githubusercontent.com/MagicAutomaton/MyImages/main/DynamicText/speed.gif">

<h1>&lt;wave&gt;</h1>

Format:&lt;wave <i>intensity</i>&gt;&lt;/wave&gt;

<i>If a wave tag is nested inside another wave tag, their effects are added. </i>

Example:

<image src="https://raw.githubusercontent.com/MagicAutomaton/MyImages/main/DynamicText/wave.gif">

<h1>&lt;shake&gt;</h1>

Format:&lt;shake <i>intensity</i>&gt;&lt;/shake&gt;

<i>If a shake tag is nested inside another shake tag, their effects are added. </i>

Example:

<image src="https://raw.githubusercontent.com/MagicAutomaton/MyImages/main/DynamicText/shake.gif">
