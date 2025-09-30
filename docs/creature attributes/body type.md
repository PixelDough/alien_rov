what model type is used for the creature. body types can contain multiple models within them. body type subtypes are determined by an associated "body sub type" index, which is passed alongside the body type attribute to children

the body type also defines what [[classification]] it is. this is used for the name construction only.

body types can also come with "implicit [[descriptor trait|descriptor traits]]" which are ONLY explicitly added to a child if the body type is *not* passed to the child (to avoid situations like swordfish + sword). when a body type has an implicit [[descriptor trait]], said trait is ALWAYS listed first, right after the [[classification]] of the creature.

examples:
- generic
	- this model contains a bunch of "generic" fish models. basic fish that would JUST be described as "fish" for example
- swordfish
	- implicit trait: "utala" (sword)
- sea robin
	- implicit trait: "noka" (leg)
- turtle
	- implicit trait: "kiwen" (shell)