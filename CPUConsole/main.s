	.org $0000	
	.word 1234
	.word $55

	.org $0800
reset:
	ldx #$ff
	txs

loop:
	jmp loop

	.org $fffc
	.word reset
	.word $0000