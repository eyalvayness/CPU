OUT = $6000

	.org $7000
message:
	.asciiz "Hello, world!"
loop:
	jmp loop

	.org $8000
reset:
	ldx #$ff
	txs
	ldx #0

print:
	lda message,x
	beq loop
	sta OUT
	inx
	jmp print

	.org $fffc
	.word reset
	.word $0000