	.org $0000	
	.word 1234
	.word $55

	.org $0800
reset:
	ldy $02
	ldx #$02
	lda $0000,x
	txs

loop:
	jmp loop

	.org $fffc
	.word reset
	.word $0000